using System.ComponentModel;

namespace VectorLang.Model;

public enum BinaryOperator
{
    [Description("+"), Format("{} + {}")] Plus,
    [Description("-"), Format("{} - {}")] Minus,
    [Description("*"), Format("{} * {}")] Multiply,
    [Description("/"), Format("{} / {}")] Divide,
    [Description("%"), Format("{} % {}")] Modulo,
}
