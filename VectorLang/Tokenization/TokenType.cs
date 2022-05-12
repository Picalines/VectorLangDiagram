using System.ComponentModel;

namespace VectorLang.Tokenization;

public enum TokenType
{
    [Description("whitespace"), TokenRegex(@"\s+")] Space,
    [Description("comment"), TokenRegex(@"\/\/.*\n?")] Comment,

    [Description("';'"), TokenRegex(@";")] Semicolon,
    [Description("','"), TokenRegex(@",")] Comma,
    [Description("'.'"), TokenRegex(@"\.")] Dot,
    [Description("'->'"), TokenRegex(@"->")] Arrow,

    [Description("'('"), TokenRegex(@"\(")] OpenParenthesis,
    [Description("')'"), TokenRegex(@"\)")] CloseParenthesis,
    [Description("'['"), TokenRegex(@"\[")] OpenSquareBracket,
    [Description("']'"), TokenRegex(@"\]")] CloseSquareBracket,
    [Description("'{'"), TokenRegex(@"\{")] OpenCurlyBrace,
    [Description("'}'"), TokenRegex(@"\}")] CloseCurlyBrace,

    [Description("'def'"), TokenRegex(@"\bdef\b")] KeywordDef,
    [Description("'const'"), TokenRegex(@"\bconst\b")] KeywordConst,
    [Description("'external'"), TokenRegex(@"\bexternal\b")] KeywordExternal,
    [Description("'val'"), TokenRegex(@"\bval\b")] KeywordVal,
    [Description("'if'"), TokenRegex(@"\bif\b")] KeywordIf,
    [Description("'else'"), TokenRegex(@"\belse\b")] KeywordElse,

    [Description("':='"), TokenRegex(@":=")] OperatorAssign,
    [Description("'<='"), TokenRegex(@"<=")] OperatorLessOrEqual,
    [Description("'<'"), TokenRegex(@"<")] OperatorLess,
    [Description("'>='"), TokenRegex(@">=")] OperatorGreaterOrEqual,
    [Description("'>'"), TokenRegex(@">")] OperatorGreater,
    [Description("'!='"), TokenRegex(@"!=")] OperatorNotEquals,
    [Description("'='"), TokenRegex(@"=")] OperatorEquals,
    [Description("'+'"), TokenRegex(@"\+")] OperatorPlus,
    [Description("'-'"), TokenRegex(@"\-")] OperatorMinus,
    [Description("'*'"), TokenRegex(@"\*")] OperatorMultiply,
    [Description("'/'"), TokenRegex(@"\/")] OperatorDivide,
    [Description("'%'"), TokenRegex(@"\%")] OperatorModulo,
    [Description("'not'"), TokenRegex(@"\bnot\b")] OperatorNot,
    [Description("'and'"), TokenRegex(@"\band\b")] OperatorAnd,
    [Description("'or'"), TokenRegex(@"\bor\b")] OperatorOr,

    [Description("number literal"), TokenRegex(@"(?<value>\d+(\.\d+)?|\.\d+)(?<unit>\w*)", true)] LiteralNumber,
    [Description("boolean literal"), TokenRegex(@"\b((?<true>true)|(?<false>false))\b", true)] LiteralBoolean,
    [Description("color literal"), TokenRegex(@"#(?<r>[a-fA-F\d]{2})(?<g>[a-fA-F\d]{2})(?<b>[a-fA-F\d]{2})", true)] LiteralColor,
    [Description("void literal"), TokenRegex(@"\bvoid\b")] LiteralVoid,

    [Description("identifier"), TokenRegex(@"\b[a-zA-Z_][a-zA-Z0-9_]*\b")] Identifier,
}
