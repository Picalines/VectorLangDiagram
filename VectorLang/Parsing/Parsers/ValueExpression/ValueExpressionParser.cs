using System;
using System.Collections.Generic;
using System.Linq;
using VectorLang.Model;
using VectorLang.SyntaxTree;
using VectorLang.Tokenization;

namespace VectorLang.Parsing;

internal static partial class ValueExpressionParser
{
    private record CallInfo(IReadOnlyList<ValueExpressionNode> Arguments, Token ClosingParenthesis);

    private static readonly Parser<ValueExpressionNode> LambdaRef = Parse.Ref(() => Lambda);

    public static readonly Parser<VariableNode> Variable =
        from token in ParseToken.Identifier
        select new VariableNode(token);

    private static readonly Parser<ValueExpressionNode> AssignedValue =
        from assignOp in ParseToken.OperatorAssign
        from valueExpression in LambdaRef
        select valueExpression;

    private static readonly Parser<ValueExpressionNode> VariableOrAssignment =
        Variable.Then(variable =>
            (AssignedValue.Select(valueExpression => new VariableAssignmentNode(variable, valueExpression)) as Parser<ValueExpressionNode>)
            .XOr(Parse.Return(variable))
        );

    private static readonly Parser<VariableCreationNode> VariableCreation =
        from valKeyword in ParseToken.KeywordVal
        from variable in Variable
        from valueExpression in AssignedValue
        select new VariableCreationNode(valKeyword, variable, valueExpression);

    private static readonly Parser<ValueExpressionNode> InnerExpression =
        from openParen in ParseToken.OpenParenthesis
        from innerExpression in LambdaRef
        from closeParen in ParseToken.CloseParenthesis
        select innerExpression;

    private static readonly Parser<BlockNode> Block =
        from openBracket in ParseToken.OpenSquareBracket
        from block in
            (
                from priorExpressions in LambdaRef.FollowedBy(ParseToken.Semicolon).Many()
                from resultExpression in LambdaRef
                from closeBracket in ParseToken.CloseSquareBracket
                select new BlockNode(priorExpressions, resultExpression, openBracket, closeBracket)
            )
            .Or(
                from expressions in LambdaRef.FollowedBy(ParseToken.Semicolon).AtLeastOnce()
                from closeBracket in ParseToken.CloseSquareBracket
                select new BlockNode(expressions, null, openBracket, closeBracket)
             )
        select block;

    private static readonly Parser<VectorNode> Vector =
        from openBrace in ParseToken.OpenCurlyBrace
        from x in LambdaRef
        from comma in ParseToken.Comma
        from y in LambdaRef
        from closeBrace in ParseToken.CloseCurlyBrace
        select new VectorNode(x, y, openBrace, closeBrace);

    private static readonly Parser<ValueExpressionNode> PrimaryTerm =
        (ConstantParser.Number as Parser<ValueExpressionNode>)
        .XOr(ConstantParser.Boolean)
        .XOr(ConstantParser.Color)
        .XOr(VariableOrAssignment)
        .XOr(Vector)
        .XOr(VariableCreation)
        .XOr(InnerExpression)
        .XOr(Block);

    private static readonly Parser<CallInfo> Call =
        from openParen in ParseToken.OpenParenthesis
        from arguments in LambdaRef.DelimitedBy(ParseToken.Comma)
        from closeParen in ParseToken.CloseParenthesis
        select new CallInfo(arguments, closeParen);

    private static readonly Parser<Token> Member =
        from dot in ParseToken.Dot
        from identifier in ParseToken.Identifier
        select identifier;

    private static readonly Parser<ValueExpressionNode> MappedTerm =
        PrimaryTerm.MaybeThen(factor =>
            (Call as Parser<object>).Or(Member).AtLeastOnce()
            .Select(rightOps => rightOps.Aggregate(
                factor,
                (left, operation) => operation switch
                {
                    CallInfo(var arguments, var closeParen) => new CalledNode(left, arguments, closeParen),
                    Token member => new MemberNode(left, member),
                    _ => throw new NotImplementedException(),
                }
            ))
        );

    private static readonly Parser<ValueExpressionNode> SignedTerm =
        ParseToken.OperatorPlus.Select(token => new { Operator = UnaryOperator.Plus, Token = token })
        .Or(ParseToken.OperatorMinus.Select(token => new { Operator = UnaryOperator.Minus, Token = token }))
        .Then(op => MappedTerm.Select(factor => new UnaryExpressionNode(factor, op.Operator, op.Token)))
        .XOr(MappedTerm);

    private static readonly Func<BinaryOperator, ValueExpressionNode, ValueExpressionNode, BinaryExpressionNode> CreateBinaryExpression =
        (op, left, right) => new BinaryExpressionNode(left, right, op);

    private static readonly Parser<ValueExpressionNode> Multiplication =
        Parse.ChainOperator(
            ParseToken.OperatorMultiply.As(BinaryOperator.Multiply)
            .Or(ParseToken.OperatorDivide.As(BinaryOperator.Divide))
            .Or(ParseToken.OperatorModulo.As(BinaryOperator.Modulo)),
            SignedTerm,
            CreateBinaryExpression
        );

    private static readonly Parser<ValueExpressionNode> Addition =
        Parse.ChainOperator(
            ParseToken.OperatorPlus.As(BinaryOperator.Plus)
            .Or(ParseToken.OperatorMinus.As(BinaryOperator.Minus)),
            Multiplication,
            CreateBinaryExpression
        );

    private static readonly Parser<ValueExpressionNode> Relation =
        Addition.MaybeThen(left =>
            ParseToken.OperatorEquals.As(BinaryOperator.Equals)
            .Or(ParseToken.OperatorNotEquals.As(BinaryOperator.NotEquals))
            .Or(ParseToken.OperatorLess.As(BinaryOperator.Less))
            .Or(ParseToken.OperatorLessOrEqual.As(BinaryOperator.LessOrEqual))
            .Or(ParseToken.OperatorGreater.As(BinaryOperator.Greater))
            .Or(ParseToken.OperatorGreaterOrEqual.As(BinaryOperator.GreaterOrEqual))
            .Then(op =>
                Addition.Select(right => new BinaryExpressionNode(left, right, op))
            )
        );


    private static readonly Parser<ValueExpressionNode> Not =
        ParseToken.OperatorNot.Then(notToken =>
            Relation.Select(factor => new UnaryExpressionNode(factor, UnaryOperator.Not, notToken))
        )
        .XOr(Relation);


    private static readonly Parser<ValueExpressionNode> And =
        Parse.ChainOperator(ParseToken.OperatorAnd.As(BinaryOperator.And), Not, CreateBinaryExpression);


    private static readonly Parser<ValueExpressionNode> BooleanExpression =
        Parse.ChainOperator(ParseToken.OperatorOr.As(BinaryOperator.Or), And, CreateBinaryExpression);

    private static readonly Parser<ValueExpressionNode> ConditionalExpression =
        from ifKeyword in ParseToken.KeywordIf
        from openParen in ParseToken.OpenParenthesis
        from condition in LambdaRef
        from closeParen in ParseToken.CloseParenthesis
        from trueValue in LambdaRef
        from falseValue in
            (
                from elseKeyword in ParseToken.KeywordElse
                from falseValue in LambdaRef
                select falseValue
            )
            .XOr(Parse.Return<ValueExpressionNode?>(null))
        select new ConditionalExpressionNode(ifKeyword, condition, trueValue, falseValue);

    public static readonly Parser<ValueExpressionNode> Lambda = ConditionalExpression.XOr(BooleanExpression);
}
