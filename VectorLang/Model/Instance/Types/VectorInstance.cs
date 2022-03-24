using System;

namespace VectorLang.Model;

internal sealed class VectorInstance : Instance
{
    public const string XFieldName = "x";

    public const string YFieldName = "y";

    public const string LengthFieldName = "length";

    public double X { get; }

    public double Y { get; }

    public VectorInstance(double x, double y) : base(VectorInstanceType.Instance)
    {
        X = x;
        Y = y;
    }

    public double Length => Math.Sqrt(X * X + Y * Y);

    protected override Instance? GetFieldInternal(string name) => name switch
    {
        XFieldName => new NumberInstance(X),
        YFieldName => new NumberInstance(Y),
        LengthFieldName => new NumberInstance(Length),
        _ => null,
    };

    public VectorInstance Normalized()
    {
        var length = Length;

        if (length == 0.0)
        {
            return this;
        }

        return new VectorInstance(X / length, Y / length);
    }

    public NumberInstance Dot(VectorInstance other)
    {
        return new NumberInstance(X * other.X + Y * other.Y);
    }

    public static VectorInstance operator +(VectorInstance right)
    {
        return right;
    }

    public static VectorInstance operator -(VectorInstance right)
    {
        return new VectorInstance(-right.X, -right.Y);
    }

    public static VectorInstance operator +(VectorInstance first, VectorInstance second)
    {
        return new VectorInstance(first.X + second.X, first.Y + second.Y);
    }

    public static VectorInstance operator -(VectorInstance first, VectorInstance second)
    {
        return new VectorInstance(first.X - second.X, first.Y - second.Y);
    }

    public static NumberInstance operator *(VectorInstance first, VectorInstance second)
    {
        return first.Dot(second);
    }

    public static VectorInstance operator *(VectorInstance vector, NumberInstance number)
    {
        return new VectorInstance(vector.X * number.Value, vector.Y * number.Value);
    }

    public static VectorInstance operator /(VectorInstance vector, NumberInstance number)
    {
        if (number.Value == 0.0)
        {
            throw new DivideByZeroException();
        }

        return new VectorInstance(vector.X / number.Value, vector.Y / number.Value);
    }
}
