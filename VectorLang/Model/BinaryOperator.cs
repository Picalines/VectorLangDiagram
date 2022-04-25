using System.ComponentModel;

namespace VectorLang.Model;

public enum BinaryOperator
{
    [Description("+"), Format("{0} + {1}")] Plus,
    [Description("-"), Format("{0} - {1}")] Minus,
    [Description("*"), Format("{0} * {1}")] Multiply,
    [Description("/"), Format("{0} / {1}")] Divide,
    [Description("%"), Format("{0} % {1}")] Modulo,
    [Description("<"), Format("{0} < {1}")] Less,
    [Description("<="), Format("{0} <= {1}")] LessOrEqual,
    [Description(">"), Format("{0} > {1}")] Greater,
    [Description(">="), Format("{0} >= {1}")] GreaterOrEqual,
    [Description("="), Format("{0} = {1}")] Equals,
    [Description("!="), Format("{0} != {1}")] NotEquals,
    [Description("and"), Format("{0} and {1}")] And,
    [Description("or"), Format("{0} or {1}")] Or,
}
