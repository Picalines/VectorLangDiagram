using System.ComponentModel;

namespace VectorLang.Model;

public enum BinaryOperator
{
    [Description("+")] Plus,
    [Description("-")] Minus,
    [Description("*")] Multiply,
    [Description("/")] Divide,
    [Description("%")] Modulo,
}
