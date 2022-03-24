namespace VectorLang.Model;

internal sealed class NumberInstance : Instance
{
    public double Value { get; }

    public NumberInstance(double value) : base(NumberInstanceType.Instance)
    {
        Value = value;
    }
}
