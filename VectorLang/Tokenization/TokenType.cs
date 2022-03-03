using System.ComponentModel;

namespace VectorLang.Tokenization;

public enum TokenType
{
    [Description("whitespace"), TokenRegex(@"\s+")] Space,
    [Description("comment"), TokenRegex(@"\/\/.*\n?")] Comment,

    [Description("comma"), TokenRegex(@",")] Comma,
    [Description("dot"), TokenRegex(@"\.")] Dot,
    [Description("arrow '->'"), TokenRegex(@"->")] Arrow,

    [Description("open parenthesis"), TokenRegex(@"\(")] OpenParenthesis,
    [Description("close parenthesis"), TokenRegex(@"\)")] CloseParenthesis,
    [Description("open brace"), TokenRegex(@"\{")] CurlyBraceOpen,
    [Description("closing brace"), TokenRegex(@"\}")] CurlyBraceClose,

    [Description("keyword 'vector'"), TokenRegex(@"\bvector\b")] KeywordVector,
    [Description("keyword 'def"), TokenRegex(@"\bdef\b")] KeywordDef,
    [Description("keyword 'external"), TokenRegex(@"\bextenal\b")] KeywordExternal,
    [Description("keyword 'val"), TokenRegex(@"\bval\b")] KeywordVal,
    [Description("keyword 'plot"), TokenRegex(@"\bplot\b")] KeywordPlot,

    [Description("operator '!='"), TokenRegex(@"!=")] OperatorNotEquals,
    [Description("operator '='"), TokenRegex(@"=")] OperatorEquals,
    [Description("operator '+'"), TokenRegex(@"\+")] OperatorPlus,
    [Description("operator '-'"), TokenRegex(@"\-")] OperatorMinus,
    [Description("operator '*'"), TokenRegex(@"\*")] OperatorMultiply,
    [Description("operator '/'"), TokenRegex(@"\/")] OperatorDivide,
    [Description("operator '%'"), TokenRegex(@"\%")] OperatorModulo,

    [Description("number literal"), TokenRegex(@"(?<value>\d+(\.\d+)?|\.\d+)(?<unit>\w*)", true)] LiteralNumber,
    [Description("string literal"), TokenRegex(@"""""|''|([""']).*?[^\\]\1")] LiteralString,
    [Description("color literal"), TokenRegex(@"#(?<r>[a-fA-F\d]{2})(?<g>[a-fA-F\d]{2})(?<b>[a-fA-F\d]{2})", true)] LiteralColor,

    [Description("identifier"), TokenRegex(@"\b[a-zA-Z_][a-zA-Z0-9_]*\b")] Identifier,
}
