using System;
using System.Collections.Generic;

namespace VectorLang.Model;

internal sealed class NumberInstance : ReflectionInstance
{
    private const int MinCachedValue = -5;

    private const int MaxCachedValue = 256;

    [ReflectionInstanceType]
    public static readonly ReflectionInstanceType InstanceType = ReflectionInstanceType.Of<NumberInstance>("number");

    private static readonly Dictionary<double, NumberInstance> _CachedInstances = new();

    public double Value { get; }

    private NumberInstance(double value) : base(InstanceType)
    {
        Value = value;
    }

    static NumberInstance()
    {
        static void AddToCache(double value)
        {
            _CachedInstances.Add(value, new NumberInstance(value));
        }

        AddToCache(0.75);
        AddToCache(0.5);
        AddToCache(0.25);
        AddToCache(Math.Sqrt(2));
        AddToCache(Math.Sqrt(3));
        AddToCache(Math.PI);
        AddToCache(Math.E);
        AddToCache(Math.Tau);
    }

    public static NumberInstance From(double value)
    {
        if (_CachedInstances.TryGetValue(value, out var instance))
        {
            return instance;
        }

        instance = new NumberInstance(value);

        if (value % 1 == 0 && value >= MinCachedValue && value <= MaxCachedValue)
        {
            _CachedInstances.Add(value, instance);
        }

        return instance;
    }

    public static implicit operator NumberInstance(double value) => From(value);

    [InstanceMethod("abs")]
    public NumberInstance Abs() => Math.Abs(Value);

    [InstanceMethod("sign")]
    public NumberInstance Sign() => Math.Sign(Value);

    [InstanceMethod("sqr")]
    public NumberInstance Sqr() => Value * Value;

    [InstanceMethod("sqrt")]
    public NumberInstance Sqrt() => Math.Sqrt(Value);

    [InstanceMethod("sin")]
    public NumberInstance Sin() => Math.Sin(Value);

    [InstanceMethod("cos")]
    public NumberInstance Cos() => Math.Cos(Value);

    [InstanceMethod("tan")]
    public NumberInstance Tan() => Math.Tan(Value);

    [InstanceMethod("cot")]
    public NumberInstance Cot() => 1.0 / Math.Tan(Value);

    [InstanceMethod("asin")]
    public NumberInstance Asin() => Math.Asin(Value);

    [InstanceMethod("acos")]
    public NumberInstance Acos() => Math.Acos(Value);

    [InstanceMethod("atan")]
    public NumberInstance Atan() => Math.Atan(Value);

    [InstanceMethod("exp")]
    public NumberInstance Exp() => Math.Exp(Value);

    [InstanceMethod("ln")]
    public NumberInstance Ln() => Math.Log(Value);

    [InstanceMethod("log")]
    public NumberInstance Log(NumberInstance logBase) => Math.Log(Value, logBase.Value);

    [InstanceMethod("lerp")]
    public NumberInstance Lerp(NumberInstance to, NumberInstance progress) => this + progress * (to - this);

    [InstanceMethod("min")]
    public NumberInstance Min(NumberInstance other) => Value < other.Value ? this : other;

    [InstanceMethod("max")]
    public NumberInstance Max(NumberInstance other) => Value > other.Value ? this : other;

    [InstanceMethod("clamp")]
    public NumberInstance Clamp(NumberInstance min, NumberInstance max) => Math.Clamp(Value, min.Value, max.Value);

    [InstanceMethod("round")]
    public NumberInstance Round() => Math.Round(Value);

    [InstanceMethod("floor")]
    public NumberInstance Floor() => Math.Floor(Value);

    [InstanceMethod("ceil")]
    public NumberInstance Ceil() => Math.Ceiling(Value);

    [InstanceMethod("isInt")]
    public BooleanInstance IsInt() => Value % 1 == 0; // may be wrong when close to epsilon

    [InstanceMethod("roundDigits")]
    public NumberInstance RoundDigits(NumberInstance digits) => Math.Round(Value, (int)digits.Value);

    [InstanceOperator]
    public static NumberInstance operator +(NumberInstance right) => right;

    [InstanceOperator]
    public static NumberInstance operator -(NumberInstance right) => -right.Value;

    [InstanceOperator]
    public static NumberInstance operator +(NumberInstance left, NumberInstance right) => left.Value + right.Value;

    [InstanceOperator]
    public static NumberInstance operator -(NumberInstance left, NumberInstance right) => left.Value - right.Value;

    [InstanceOperator]
    public static NumberInstance operator *(NumberInstance left, NumberInstance right) => left.Value * right.Value;

    [InstanceOperator]
    public static NumberInstance operator /(NumberInstance left, NumberInstance right)
    {
        if (right.Value == 0.0)
        {
            throw RuntimeException.ZeroDivision();
        }

        return left.Value / right.Value;
    }

    [InstanceOperator]
    public static NumberInstance operator %(NumberInstance left, NumberInstance right)
    {
        if (right.Value == 0.0)
        {
            throw RuntimeException.ZeroDivision();
        }

        return left.Value % right.Value;
    }

    [InstanceOperator]
    public static VectorInstance operator *(NumberInstance number, VectorInstance vector) => vector * number;

    [InstanceOperator]
    public static VectorInstance operator /(NumberInstance number, VectorInstance vector) => vector / number;

    [InstanceOperator]
    public static BooleanInstance operator <(NumberInstance left, NumberInstance right) => left.Value < right.Value;

    [InstanceOperator]
    public static BooleanInstance operator <=(NumberInstance left, NumberInstance right) => left.Value <= right.Value;

    [InstanceOperator]
    public static BooleanInstance operator >(NumberInstance left, NumberInstance right) => left.Value > right.Value;

    [InstanceOperator]
    public static BooleanInstance operator >=(NumberInstance left, NumberInstance right) => left.Value >= right.Value;

    [InstanceOperator]
    public static BooleanInstance operator ==(NumberInstance left, NumberInstance right) => left.Value == right.Value;

    [InstanceOperator]
    public static BooleanInstance operator !=(NumberInstance left, NumberInstance right) => left.Value != right.Value;
}
