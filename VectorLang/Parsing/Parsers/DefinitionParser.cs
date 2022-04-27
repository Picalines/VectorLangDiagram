using VectorLang.SyntaxTree;

namespace VectorLang.Parsing;

internal static class DefinitionParser
{
    public static readonly Parser<ConstantDefinition> Constant =
        from constKeyword in ParseToken.KeywordConst
        from name in ParseToken.Identifier.Named("name of constant")
        from equalsOp in ParseToken.OperatorEquals
        from valueExpression in ValueExpressionParser.Lambda.Named("value of constant")
        from semicolon in ParseToken.Semicolon
        select new ConstantDefinition(name, equalsOp, semicolon, valueExpression);

    public static readonly Parser<ExternalValueDefinition> ExternalValue =
        from externalKeyword in ParseToken.KeywordExternal
        from type in TypeParser.Type.Named("type of external value")
        from name in ParseToken.Identifier.Named("name of external value")
        from semicolon in ParseToken.Semicolon
        select new ExternalValueDefinition(type, name, semicolon);

    public static readonly Parser<ArgumentDefinition> Argument =
        from type in TypeParser.Type.Named("argument type")
        from name in ParseToken.Identifier.Named("argument name")
        select new ArgumentDefinition(type, name.Value);

    public static readonly Parser<FunctionDefinition> Function =
        from defKeyword in ParseToken.KeywordDef
        from name in ParseToken.Identifier.Named("function name")
        from openParen in ParseToken.OpenParenthesis
        from arguments in Argument.DelimitedBy(ParseToken.Comma)
        from closeParen in ParseToken.CloseParenthesis.Named("argument list")
        from arrow in ParseToken.Arrow
        from returnType in TypeParser.Type.Named("function return type")
        from equalsOp in ParseToken.OperatorEquals
        from expression in ValueExpressionParser.Lambda.Named("function value expression")
        from semicolon in ParseToken.Semicolon
        select new FunctionDefinition(name, equalsOp, semicolon, returnType, arguments, expression);
}
