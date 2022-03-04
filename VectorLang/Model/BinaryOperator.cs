using System.ComponentModel;

namespace VectorLang.Model;

public enum BinaryOperator
{
    [Description("+")] Add,
    [Description("-")] Subtract,
    [Description("*")] Multiply,
    [Description("/")] Divide,
    [Description("%")] Modulo,
}
