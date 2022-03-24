﻿namespace VectorLang.Model;

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

        DefineOperator(BinaryOperator.Add, this, this, (NumberInstance a, NumberInstance b) => new NumberInstance(a.Value + b.Value));
        DefineOperator(BinaryOperator.Subtract, this, this, (NumberInstance a, NumberInstance b) => new NumberInstance(a.Value - b.Value));
        DefineOperator(BinaryOperator.Multiply, this, this, (NumberInstance a, NumberInstance b) => new NumberInstance(a.Value * b.Value));

        // TODO: zero division?
        DefineOperator(BinaryOperator.Divide, this, this, (NumberInstance a, NumberInstance b) => new NumberInstance(a.Value / b.Value));
        DefineOperator(BinaryOperator.Modulo, this, this, (NumberInstance a, NumberInstance b) => new NumberInstance(a.Value % b.Value));
    }
}
