using System.ComponentModel;

namespace VectorLang.Model;

public enum BinaryOperator
{
    [Description("+"), Format("{0} + {1}")] Plus,
    [Description("-"), Format("{0} - {1}")] Minus,
    [Description("*"), Format("{0} * {1}")] Multiply,
    [Description("/"), Format("{0} / {1}")] Divide,
    [Description("%"), Format("{0} % {1}")] Modulo,
}
