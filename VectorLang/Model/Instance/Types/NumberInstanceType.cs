using System;

namespace VectorLang.Model;

internal sealed class NumberInstance : Instance
{
    public double Value { get; }

    public NumberInstance(double value) : base(NumberInstanceType.Instance)
    {
        Value = value;
    }
}

internal sealed class NumberInstanceType : InstanceType
{
    public static readonly NumberInstanceType Instance = new();

    private NumberInstanceType() : base("number")
    {
    }

    protected override void DefineMembersInternal()
    {
        DefineOperator(UnaryOperator.Plus, this, thisInstance => thisInstance);
        DefineOperator(UnaryOperator.Minus, this, (NumberInstance thisInstance) => new NumberInstance(-thisInstance.Value));

        DefineOperator(BinaryOperator.Plus, this, this, (NumberInstance a, NumberInstance b) => new NumberInstance(a.Value + b.Value));
        DefineOperator(BinaryOperator.Minus, this, this, (NumberInstance a, NumberInstance b) => new NumberInstance(a.Value - b.Value));
        DefineOperator(BinaryOperator.Multiply, this, this, (NumberInstance a, NumberInstance b) => new NumberInstance(a.Value * b.Value));

        DefineOperator(BinaryOperator.Divide, this, this, (NumberInstance a, NumberInstance b) =>
        {
            AssertZeroDivision(b.Value);
            return new NumberInstance(a.Value / b.Value);
        });

        DefineOperator(BinaryOperator.Modulo, this, this, (NumberInstance a, NumberInstance b) =>
        {
            AssertZeroDivision(b.Value);
            return new NumberInstance(a.Value % b.Value);
        });
    }

    private static void AssertZeroDivision(double denominator)
    {
        if (denominator == 0.0)
        {
            throw new DivideByZeroException();
        }
    }
}
