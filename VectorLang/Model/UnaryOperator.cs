using System.ComponentModel;

namespace VectorLang.Model;

public enum UnaryOperator
{
    [Description("+"), Format("+{}")] Plus,
    [Description("-"), Format("-{}")] Minus,
}
