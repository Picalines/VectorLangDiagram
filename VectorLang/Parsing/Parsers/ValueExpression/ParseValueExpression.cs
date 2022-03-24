using System;
using System.Collections.Generic;
using System.Linq;
using VectorLang.Model;
using VectorLang.SyntaxTree;
using VectorLang.Tokenization;

namespace VectorLang.Parsing;

internal static partial class ParseValueExpression
{
    private record CallInfo(IReadOnlyList<ValueExpressionNode> Arguments, Token ClosingParenthesis);

    public static readonly Parser<VariableNode> Variable =
        from token in ParseToken.Identifier
        select new VariableNode(token);

    private static readonly Parser<ValueExpressionNode> InnerExpression =
        from openParen in ParseToken.OpenParenthesis
        from innerExpression in Parse.Ref(() => Lambda)
        from closeParen in ParseToken.CloseParenthesis
        select innerExpression;

    private static readonly Parser<ValueExpressionNode> PrimaryTerm =
        (ParseConstant.Number as Parser<ValueExpressionNode>)
        .Or(ParseConstant.String)
        .Or(ParseConstant.Color)
        .Or(Variable)
        .Or(InnerExpression)
        .WithError("expression term expected");

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
        ParseToken.OperatorPlus.With(UnaryOperator.Plus)
        .Or(ParseToken.OperatorMinus.With(UnaryOperator.Minus))
        .Then(op => MappedTerm.Select(factor => new UnaryExpressionNode(factor, op.Item2, op.Item1)))
        .Or(MappedTerm);

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
