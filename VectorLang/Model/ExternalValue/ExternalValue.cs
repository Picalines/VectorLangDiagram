namespace VectorLang.Model;

public abstract class ExternalValue
{
    internal Instance ValueInstance { get; private protected set; }

    internal ExternalValue(Instance value)
    {
        ValueInstance = value;
    }
}
