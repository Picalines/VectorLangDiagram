using System;

namespace VectorLang.Model;

internal sealed class VectorInstance : ReflectionInstance
{
    [ReflectionInstanceType]
    public static readonly ReflectionInstanceType InstanceType = ReflectionInstanceType.Of<VectorInstance>("vector");

    [InstanceField("x")]
    public NumberInstance X { get; }

    [InstanceField("y")]
    public NumberInstance Y { get; }

    [InstanceField("length")]
    public NumberInstance Length { get; }

    public VectorInstance(NumberInstance x, NumberInstance y) : base(InstanceType)
    {
        X = x;
        Y = y;

        Length = (X * X + Y * Y).Sqrt();
    }

    public VectorInstance(double x, double y)
        : this(NumberInstance.From(x), NumberInstance.From(y))
    {
    }

    public (double X, double Y) ToTuple() => (X.Value, Y.Value);

    [InstanceMethod("normalize")]
    public VectorInstance Normalize() => Length.Value > 0.0 ? new VectorInstance(X / Length, Y / Length) : this;

    [InstanceMethod("dot")]
    public NumberInstance Dot(VectorInstance other) => X * other.X + Y * other.Y;

    [InstanceMethod("angleCos")]
    public NumberInstance AngleCos(VectorInstance other) => Dot(other) / (Length * other.Length);

    [InstanceMethod("angle")]
    public NumberInstance Angle(VectorInstance other) => AngleCos(other).Acos();

    [InstanceMethod("lerp")]
    public VectorInstance Lerp(VectorInstance to, NumberInstance progress) => new(X.Lerp(to.X, progress), Y.Lerp(to.Y, progress));

    [InstanceMethod("clampLength")]
    public VectorInstance ClampLength(NumberInstance minLength, NumberInstance maxLength) => Normalize() * Length.Clamp(minLength, maxLength);

    [InstanceMethod("rotate")]
    public VectorInstance Rotate(NumberInstance angle) => new(
        X * angle.Cos() - Y * angle.Sin(),
        X * angle.Sin() + Y * angle.Cos()
    );

    [InstanceMethod("scale")]
    public VectorInstance Scale(VectorInstance scale) => new(X * scale.X, Y * scale.Y);

    [InstanceOperator]
    public static VectorInstance operator +(VectorInstance right) => right;

    [InstanceOperator]
    public static VectorInstance operator -(VectorInstance right) => new(-right.X, -right.Y);

    [InstanceOperator]
    public static VectorInstance operator +(VectorInstance first, VectorInstance second) => new(first.X + second.X, first.Y + second.Y);

    [InstanceOperator]
    public static VectorInstance operator -(VectorInstance first, VectorInstance second) => new(first.X - second.X, first.Y - second.Y);

    [InstanceOperator]
    public static NumberInstance operator *(VectorInstance first, VectorInstance second) => first.Dot(second);

    [InstanceOperator]
    public static VectorInstance operator *(VectorInstance vector, NumberInstance number) => new(vector.X * number, vector.Y * number);

    [InstanceOperator]
    public static VectorInstance operator /(VectorInstance vector, NumberInstance number)
    {
        if (number.Value == 0.0)
        {
            throw new DivideByZeroException();
        }

        return new(vector.X / number, vector.Y / number);
    }

    [InstanceOperator]
    public static BooleanInstance operator ==(VectorInstance left, VectorInstance right)
    {
        var xEquality = left.X == right.Y;
        var yEquality = left.Y == right.Y;

        return xEquality.Value && yEquality.Value;
    }

    [InstanceOperator]
    public static BooleanInstance operator !=(VectorInstance left, VectorInstance right)
    {
        return !(left == right);
    }
}
