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

    [InstanceMethod("sqr")]
    public NumberInstance Sqr() => new(Value * Value);

    [InstanceMethod("sqrt")]
    public NumberInstance Sqrt() => new(Math.Sqrt(Value));

    [InstanceMethod("sin")]
    public NumberInstance Sin() => new(Math.Sin(Value));

    [InstanceMethod("cos")]
    public NumberInstance Cos() => new(Math.Cos(Value));

    [InstanceMethod("tan")]
    public NumberInstance Tan() => new(Math.Tan(Value));

    [InstanceMethod("cot")]
    public NumberInstance Cot() => new(1.0 / Math.Tan(Value));

    [InstanceMethod("asin")]
    public NumberInstance Asin() => new(Math.Asin(Value));

    [InstanceMethod("acos")]
    public NumberInstance Acos() => new(Math.Acos(Value));

    [InstanceMethod("atan")]
    public NumberInstance Atan() => new(Math.Atan(Value));

    [InstanceMethod("lerp")]
    public NumberInstance Lerp(NumberInstance to, NumberInstance progress) => this + progress * (to - this);

    [InstanceMethod("min")]
    public NumberInstance Min(NumberInstance other) => Value < other.Value ? this : other;

    [InstanceMethod("max")]
    public NumberInstance Max(NumberInstance other) => Value > other.Value ? this : other;

    [InstanceMethod("clamp")]
    public NumberInstance Clamp(NumberInstance min, NumberInstance max) => Max(min).Min(max);

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
