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

    public static readonly Parser<VariableNode> Variable =
        from token in ParseToken.Identifier
        select new VariableNode(token);

    private static readonly Parser<VariableCreationNode> VariableCreation =
        from valKeyword in ParseToken.KeywordVal
        from variable in Variable
        from assignOp in ParseToken.OperatorAssign
        from valueExpression in Parse.Ref(() => Lambda)
        select new VariableCreationNode(valKeyword, variable, valueExpression);

    private static readonly Parser<ValueExpressionNode> InnerExpression =
        from openParen in ParseToken.OpenParenthesis
        from innerExpression in Parse.Ref(() => Lambda)
        from closeParen in ParseToken.CloseParenthesis
        select innerExpression;

    private static readonly Parser<BlockNode> Block =
        from openBracket in ParseToken.OpenSquareBracket
        from block in
            (
                from priorExpressions in Parse.Ref(() => Lambda).FollowedBy(ParseToken.Semicolon).Many()
                from resultExpression in Parse.Ref(() => Lambda)
                from closeBracket in ParseToken.CloseSquareBracket
                select new BlockNode(priorExpressions, resultExpression, openBracket, closeBracket)
            )
            .Or(
                from expressions in Parse.Ref(() => Lambda).FollowedBy(ParseToken.Semicolon).AtLeastOnce()
                from closeBracket in ParseToken.CloseSquareBracket
                select new BlockNode(expressions, null, openBracket, closeBracket)
             )
        select block;

    private static readonly Parser<VectorNode> Vector =
        from openBrace in ParseToken.OpenCurlyBrace
        from x in Parse.Ref(() => Lambda)
        from comma in ParseToken.Comma
        from y in Parse.Ref(() => Lambda)
        from closeBrace in ParseToken.CloseCurlyBrace
        select new VectorNode(x, y, openBrace, closeBrace);

    private static readonly Parser<ValueExpressionNode> PrimaryTerm =
        (ConstantParser.Number as Parser<ValueExpressionNode>)
        .XOr(ConstantParser.Color)
        .XOr(Variable)
        .XOr(Vector)
        .XOr(VariableCreation)
        .XOr(InnerExpression)
        .XOr(Block);

    private static readonly Parser<CallInfo> Call =
        from openParen in ParseToken.OpenParenthesis
        from arguments in Parse.Ref(() => Lambda).DelimitedBy(ParseToken.Comma)
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

    public static readonly Parser<ValueExpressionNode> Lambda = Addition;
}
