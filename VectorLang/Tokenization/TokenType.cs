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

    [Description("keyword 'def'"), TokenRegex(@"\bdef\b")] KeywordDef,
    [Description("keyword 'external'"), TokenRegex(@"\bextenal\b")] KeywordExternal,
    [Description("keyword 'val'"), TokenRegex(@"\bval\b")] KeywordVal,
    [Description("keyword 'plot'"), TokenRegex(@"\bplot\b")] KeywordPlot,

    [Description("'!='"), TokenRegex(@"!=")] OperatorNotEquals,
    [Description("'='"), TokenRegex(@"=")] OperatorEquals,
    [Description("'+'"), TokenRegex(@"\+")] OperatorPlus,
    [Description("'-'"), TokenRegex(@"\-")] OperatorMinus,
    [Description("'*'"), TokenRegex(@"\*")] OperatorMultiply,
    [Description("'/'"), TokenRegex(@"\/")] OperatorDivide,
    [Description("'%'"), TokenRegex(@"\%")] OperatorModulo,

    [Description("number literal"), TokenRegex(@"(?<value>\d+(\.\d+)?|\.\d+)(?<unit>\w*)", true)] LiteralNumber,
    [Description("string literal"), TokenRegex(@"""""|''|([""']).*?[^\\]\1")] LiteralString,
    [Description("color literal"), TokenRegex(@"#(?<r>[a-fA-F\d]{2})(?<g>[a-fA-F\d]{2})(?<b>[a-fA-F\d]{2})", true)] LiteralColor,

    [Description("identifier"), TokenRegex(@"\b[a-zA-Z_][a-zA-Z0-9_]*\b")] Identifier,
}
