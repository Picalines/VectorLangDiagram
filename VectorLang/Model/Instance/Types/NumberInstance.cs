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

    [InstanceMethod("abs")]
    public NumberInstance Abs() => new(Math.Abs(Value));

    [InstanceMethod("sign")]
    public NumberInstance Sign() => new(Math.Sign(Value));

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

    [InstanceMethod("exp")]
    public NumberInstance Exp() => new(Math.Exp(Value));

    [InstanceMethod("ln")]
    public NumberInstance Ln() => new(Math.Log(Value));

    [InstanceMethod("log")]
    public NumberInstance Log(NumberInstance newBase) => new(Math.Log(Value, newBase.Value));

    [InstanceMethod("lerp")]
    public NumberInstance Lerp(NumberInstance to, NumberInstance progress) => this + progress * (to - this);

    [InstanceMethod("min")]
    public NumberInstance Min(NumberInstance other) => Value < other.Value ? this : other;

    [InstanceMethod("max")]
    public NumberInstance Max(NumberInstance other) => Value > other.Value ? this : other;

    [InstanceMethod("clamp")]
    public NumberInstance Clamp(NumberInstance min, NumberInstance max) => new(Math.Clamp(Value, min.Value, max.Value));

    [InstanceMethod("round")]
    public NumberInstance Round() => new(Math.Round(Value));

    [InstanceMethod("floor")]
    public NumberInstance Floor() => new(Math.Floor(Value));

    [InstanceMethod("ceil")]
    public NumberInstance Ceil() => new(Math.Ceiling(Value));

    [InstanceMethod("isInt")]
    public BooleanInstance IsInt() => BooleanInstance.From(Value % 1 == 0); // may be wrong when close to epsilon

    [InstanceMethod("roundDigits")]
    public NumberInstance RoundDigits(NumberInstance digits) => new(Math.Round(Value, (int)digits.Value));

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

    [InstanceOperator]
    public static BooleanInstance operator <(NumberInstance left, NumberInstance right) => BooleanInstance.From(left.Value < right.Value);

    [InstanceOperator]
    public static BooleanInstance operator <=(NumberInstance left, NumberInstance right) => BooleanInstance.From(left.Value <= right.Value);

    [InstanceOperator]
    public static BooleanInstance operator >(NumberInstance left, NumberInstance right) => BooleanInstance.From(left.Value > right.Value);

    [InstanceOperator]
    public static BooleanInstance operator >=(NumberInstance left, NumberInstance right) => BooleanInstance.From(left.Value >= right.Value);

    [InstanceOperator]
    public static BooleanInstance operator ==(NumberInstance left, NumberInstance right) => BooleanInstance.From(left.Value == right.Value);

    [InstanceOperator]
    public static BooleanInstance operator !=(NumberInstance left, NumberInstance right) => BooleanInstance.From(left.Value != right.Value);
}
