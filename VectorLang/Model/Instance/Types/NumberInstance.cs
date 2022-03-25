using System;

namespace VectorLang.Model;

internal sealed class NumberInstance : Instance
{
    public double Value { get; }

    public NumberInstance(double value) : base(NumberInstanceType.Instance)
    {
        Value = value;
    }

    public static NumberInstance operator +(NumberInstance right)
    {
        return right;
    }

    public static NumberInstance operator -(NumberInstance right)
    {
        return new NumberInstance(-right.Value);
    }

    public static NumberInstance operator +(NumberInstance left, NumberInstance right)
    {
        return new NumberInstance(left.Value + right.Value);
    }

    public static NumberInstance operator -(NumberInstance left, NumberInstance right)
    {
        return new NumberInstance(left.Value - right.Value);
    }

    public static NumberInstance operator *(NumberInstance left, NumberInstance right)
    {
        return new NumberInstance(left.Value * right.Value);
    }

    public static NumberInstance operator /(NumberInstance left, NumberInstance right)
    {
        if (right.Value == 0.0)
        {
            throw new DivideByZeroException();
        }

        return new NumberInstance(left.Value / right.Value);
    }

    public static NumberInstance operator %(NumberInstance left, NumberInstance right)
    {
        if (right.Value == 0.0)
        {
            throw new DivideByZeroException();
        }

        return new NumberInstance(left.Value % right.Value);
    }

    public static VectorInstance operator *(NumberInstance number, VectorInstance vector)
    {
        return vector * number;
    }

    public static VectorInstance operator /(NumberInstance number, VectorInstance vector)
    {
        return vector / number;
    }
}
