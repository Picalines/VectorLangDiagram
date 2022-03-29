using System;

namespace VectorLang.Model;

internal sealed class NumberInstance : ReflectionInstance
{
    [ReflectionInstanceType]
    public static readonly ReflectionInstanceType InstanceType = ReflectionInstanceType.Of<NumberInstance>("number");

    public double Value { get; }

    public NumberInstance(double value) : base(InstanceType)
    {
        Value = value;
    }

    [InstanceOperator]
    public static NumberInstance operator +(NumberInstance right) => right;

    [InstanceOperator]
    public static NumberInstance operator -(NumberInstance right) => new(-right.Value);

    [InstanceOperator]
    public static NumberInstance operator +(NumberInstance left, NumberInstance right) => new(left.Value + right.Value);

    [InstanceOperator]
    public static NumberInstance operator -(NumberInstance left, NumberInstance right) => new(left.Value - right.Value);

    [InstanceOperator]
    public static NumberInstance operator *(NumberInstance left, NumberInstance right) => new(left.Value * right.Value);

    [InstanceOperator]
    public static NumberInstance operator /(NumberInstance left, NumberInstance right)
    {
        if (right.Value == 0.0)
        {
            throw new DivideByZeroException();
        }

        return new(left.Value / right.Value);
    }

    [InstanceOperator]
    public static NumberInstance operator %(NumberInstance left, NumberInstance right)
    {
        if (right.Value == 0.0)
        {
            throw new DivideByZeroException();
        }

        return new(left.Value % right.Value);
    }

    [InstanceOperator]
    public static VectorInstance operator *(NumberInstance number, VectorInstance vector) => vector * number;

    [InstanceOperator]
    public static VectorInstance operator /(NumberInstance number, VectorInstance vector) => vector / number;
}
