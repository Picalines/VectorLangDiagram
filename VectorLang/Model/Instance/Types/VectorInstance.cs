using System;

namespace VectorLang.Model;

internal sealed class VectorInstance : ReflectionInstance
{
    [ReflectionInstanceType]
    public static readonly ReflectionInstanceType InstanceType = ReflectionInstanceType.Of<VectorInstance>("vector");

    public double X { get; }

    public double Y { get; }

    public double Length { get; }

    [InstanceField("x")]
    public NumberInstance XInstance { get; }

    [InstanceField("y")]
    public NumberInstance YInstance { get; }

    [InstanceField("length")]
    public NumberInstance LengthInstance { get; }

    public VectorInstance(NumberInstance x, NumberInstance y) : base(InstanceType)
    {
        X = x.Value;
        Y = y.Value;

        XInstance = x;
        YInstance = y;

        Length = Math.Sqrt(X * X + Y * Y); // TODO: lazy
        LengthInstance = new(Length);
    }

    public VectorInstance(double x, double y) : this(new NumberInstance(x), new NumberInstance(y))
    {
    }

    [InstanceMethod("normalized")]
    public VectorInstance Normalized()
    {
        var length = Length;

        if (length == 0.0)
        {
            return this;
        }

        return new VectorInstance(X / length, Y / length);
    }

    [InstanceMethod("dot")]
    public NumberInstance Dot(VectorInstance other)
    {
        return new NumberInstance(X * other.X + Y * other.Y);
    }

    [InstanceOperator]
    public static VectorInstance operator +(VectorInstance right)
    {
        return right;
    }

    [InstanceOperator]
    public static VectorInstance operator -(VectorInstance right)
    {
        return new VectorInstance(-right.X, -right.Y);
    }

    [InstanceOperator]
    public static VectorInstance operator +(VectorInstance first, VectorInstance second)
    {
        return new VectorInstance(first.X + second.X, first.Y + second.Y);
    }

    [InstanceOperator]
    public static VectorInstance operator -(VectorInstance first, VectorInstance second)
    {
        return new VectorInstance(first.X - second.X, first.Y - second.Y);
    }

    [InstanceOperator]
    public static NumberInstance operator *(VectorInstance first, VectorInstance second)
    {
        return first.Dot(second);
    }

    [InstanceOperator]
    public static VectorInstance operator *(VectorInstance vector, NumberInstance number)
    {
        return new VectorInstance(vector.X * number.Value, vector.Y * number.Value);
    }

    [InstanceOperator]
    public static VectorInstance operator /(VectorInstance vector, NumberInstance number)
    {
        if (number.Value == 0.0)
        {
            throw new DivideByZeroException();
        }

        return new VectorInstance(vector.X / number.Value, vector.Y / number.Value);
    }
}
