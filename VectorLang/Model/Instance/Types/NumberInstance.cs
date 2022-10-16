using System;
using System.Collections.Generic;

namespace VectorLang.Model;

/// <vl-doc>
/// <summary>
/// Type that represents a real number
/// </summary>
/// <example>
/// let x = 123;
/// let xDeg = 45deg; // converted to pi/4 radiands
/// let xRad = 1rad; // same as just "1". Use to indicate that the value is an angle
/// </example>
/// </vl-doc>
[ReflectionInstanceType("number")]
internal sealed class NumberInstance : ReflectionInstance<NumberInstance>
{
    private const int MinCachedValue = -5;

    private const int MaxCachedValue = 256;

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

    /// <vl-doc>
    /// <returns>absolute value of number</returns>
    /// <example>
    /// 123.abs() // 123
    /// (-123).abs() // 123
    /// </example>
    /// </vl-doc>
    [InstanceMethod("abs")]
    public NumberInstance Abs() => Math.Abs(Value);

    /// <vl-doc>
    /// <returns>a sign (-1, 0, 1) of number</returns>
    /// </vl-doc>
    [InstanceMethod("sign")]
    public NumberInstance Sign() => Math.Sign(Value);

    /// <vl-doc>
    /// <returns>a squared number</returns>
    /// <example>
    /// (-2).sqr() // 4
    /// </example>
    /// </vl-doc>
    [InstanceMethod("sqr")]
    public NumberInstance Sqr() => Value * Value;

    /// <vl-doc>
    /// <returns>square root of number</returns>
    /// </vl-doc>
    [InstanceMethod("sqrt")]
    public NumberInstance Sqrt() => Math.Sqrt(Value);

    /// <vl-doc>
    /// <returns>number raised to <paramref name="power"/></returns>
    /// <param name="power">power of number</param>
    /// <example>
    /// 2.pow(4) // 16
    /// </example>
    /// </vl-doc>
    [InstanceMethod("pow")]
    public NumberInstance Pow(NumberInstance power) => Math.Pow(Value, power.Value);

    /// <vl-doc>
    /// <summary>converts degrees to radians</summary>
    /// </vl-doc>
    [InstanceMethod("degToRad")]
    public NumberInstance DegToRad() => Value * (Math.PI / 180.0);

    /// <vl-doc>
    /// <summary>converts radiands to degrees</summary>
    /// </vl-doc>
    [InstanceMethod("radToDeg")]
    public NumberInstance RadToDeg() => Value / Math.PI * 180.0;

    /// <vl-doc>
    /// <returns>sine of number</returns>
    /// </vl-doc>
    [InstanceMethod("sin")]
    public NumberInstance Sin() => Math.Sin(Value);

    /// <vl-doc>
    /// <returns>cosine of number</returns>
    /// </vl-doc>
    [InstanceMethod("cos")]
    public NumberInstance Cos() => Math.Cos(Value);

    /// <vl-doc>
    /// <returns>tangent of number</returns>
    /// </vl-doc>
    [InstanceMethod("tan")]
    public NumberInstance Tan() => Math.Tan(Value);

    // TODO: lookup proper names
    /// <vl-doc>
    /// <returns>cot of number</returns>
    /// </vl-doc>
    [InstanceMethod("cot")]
    public NumberInstance Cot() => 1.0 / Math.Tan(Value);

    /// <vl-doc>
    /// <returns>arc sine of number</returns>
    /// </vl-doc>
    [InstanceMethod("asin")]
    public NumberInstance Asin() => Math.Asin(Value);

    /// <vl-doc>
    /// <returns>arc cosing of number</returns>
    /// </vl-doc>
    [InstanceMethod("acos")]
    public NumberInstance Acos() => Math.Acos(Value);

    /// <vl-doc>
    /// <returns>arc tangent of number</returns>
    /// </vl-doc>
    [InstanceMethod("atan")]
    public NumberInstance Atan() => Math.Atan(Value);

    /// <vl-doc>
    /// <returns>e constant raised to the power of current number</returns>
    /// </vl-doc>
    [InstanceMethod("exp")]
    public NumberInstance Exp() => Math.Exp(Value);

    /// <vl-doc>
    /// <returns>natural (base e) log of number</returns>
    /// </vl-doc>
    [InstanceMethod("ln")]
    public NumberInstance Ln() => Math.Log(Value);

    /// <vl-doc>
    /// <returns>log of number in <paramref name="logBase"/></returns>
    /// <param name="logBase">log base</param>
    /// </vl-doc>
    [InstanceMethod("log")]
    public NumberInstance Log(NumberInstance logBase) => Math.Log(Value, logBase.Value);

    /// <vl-doc>
    /// <returns>number linearly interpolated between the current number and <paramref name="to"/> by <paramref name="progress"/></returns>
    /// <param name="to">target number</param>
    /// <param name="progress">0 - current number, .., 1 - target number</param>
    /// </vl-doc>
    [InstanceMethod("lerp")]
    public NumberInstance Lerp(NumberInstance to, NumberInstance progress) => this + progress * (to - this);

    /// <vl-doc>
    /// <returns>min of current number and <paramref name="other"/></returns>
    /// <param name="other">other number</param>
    /// </vl-doc>
    [InstanceMethod("min")]
    public NumberInstance Min(NumberInstance other) => Value < other.Value ? this : other;

    /// <vl-doc>
    /// <returns>max of current number and <paramref name="other"/></returns>
    /// <param name="other">other number</param>
    /// </vl-doc>
    [InstanceMethod("max")]
    public NumberInstance Max(NumberInstance other) => Value > other.Value ? this : other;

    /// <vl-doc>
    /// <returns>number clamped between <paramref name="min"/> and <paramref name="max"/></returns>
    /// <param name="min">lower bound</param>
    /// <param name="max">upper bound</param>
    /// </vl-doc>
    [InstanceMethod("clamp")]
    public NumberInstance Clamp(NumberInstance min, NumberInstance max) => Math.Clamp(Value, min.Value, max.Value);

    /// <vl-doc>
    /// <returns>closest integer number</returns>
    /// </vl-doc>
    [InstanceMethod("round")]
    public NumberInstance Round() => Math.Round(Value);

    /// <vl-doc>
    /// <returns>biggest integer number that is less than the current number</returns>
    /// </vl-doc>
    [InstanceMethod("floor")]
    public NumberInstance Floor() => Math.Floor(Value);

    /// <vl-doc>
    /// <returns>smallest integer number that is bigger than the current number</returns>
    /// </vl-doc>
    [InstanceMethod("ceil")]
    public NumberInstance Ceil() => Math.Ceiling(Value);

    /// <vl-doc>
    /// <returns>true if the number is integer</returns>
    /// </vl-doc>
    [InstanceMethod("isInt")]
    public BooleanInstance IsInt() => Value % 1 == 0; // may be wrong when close to epsilon

    /// <vl-doc>
    /// <returns>rounds current number with specified number of digits</returns>
    /// </vl-doc>
    [InstanceMethod("roundDigits")]
    public NumberInstance RoundDigits(NumberInstance digits) => Math.Round(Value, (int)digits.Value);

    /// <vl-doc>
    /// <returns>returns the current number (exists for simmetry)</returns>
    /// </vl-doc>
    [InstanceOperator]
    public static NumberInstance operator +(NumberInstance right) => right;

    /// <vl-doc>
    /// <returns>current number with the opposite sign</returns>
    /// </vl-doc>
    [InstanceOperator]
    public static NumberInstance operator -(NumberInstance right) => -right.Value;

    /// <vl-doc>
    /// <returns>the sum of two numbers</returns>
    /// </vl-doc>
    [InstanceOperator]
    public static NumberInstance operator +(NumberInstance left, NumberInstance right) => left.Value + right.Value;

    /// <vl-doc>
    /// <returns>the difference between the two numbers</returns>
    /// </vl-doc>
    [InstanceOperator]
    public static NumberInstance operator -(NumberInstance left, NumberInstance right) => left.Value - right.Value;

    /// <vl-doc>
    /// <returns>multiplication product of two numbers</returns>
    /// </vl-doc>
    [InstanceOperator]
    public static NumberInstance operator *(NumberInstance left, NumberInstance right) => left.Value * right.Value;

    /// <vl-doc>
    /// <returns>division result of two numbers. Crashes the program on 0 denominator</returns>
    /// </vl-doc>
    [InstanceOperator]
    public static NumberInstance operator /(NumberInstance left, NumberInstance right)
    {
        if (right.Value == 0.0)
        {
            throw RuntimeException.ZeroDivision();
        }

        return left.Value / right.Value;
    }

    /// <vl-doc>
    /// <returns>remainder of division of two numbers. Crashes the program on 0 denominator</returns>
    /// </vl-doc>
    [InstanceOperator]
    public static NumberInstance operator %(NumberInstance left, NumberInstance right)
    {
        if (right.Value == 0.0)
        {
            throw RuntimeException.ZeroDivision();
        }

        return left.Value % right.Value;
    }

    /// <vl-doc>
    /// <returns>vector multiplied by a scalar</returns>
    /// </vl-doc>
    [InstanceOperator]
    public static VectorInstance operator *(NumberInstance number, VectorInstance vector) => vector * number;

    /// <vl-doc>
    /// <returns>vector divided by a scalar</returns>
    /// </vl-doc>
    [InstanceOperator]
    public static VectorInstance operator /(NumberInstance number, VectorInstance vector) => vector / number;

    /// <vl-doc>
    /// <returns>true for number less than the other number</returns>
    /// </vl-doc>
    [InstanceOperator]
    public static BooleanInstance operator <(NumberInstance left, NumberInstance right) => left.Value < right.Value;

    /// <vl-doc>
    /// <returns>true for number less than or equal to the other number</returns>
    /// </vl-doc>
    [InstanceOperator]
    public static BooleanInstance operator <=(NumberInstance left, NumberInstance right) => left.Value <= right.Value;

    /// <vl-doc>
    /// <returns>true for number greater than the other number</returns>
    /// </vl-doc>
    [InstanceOperator]
    public static BooleanInstance operator >(NumberInstance left, NumberInstance right) => left.Value > right.Value;

    /// <vl-doc>
    /// <returns>true for number greater than or equal to the other number</returns>
    /// </vl-doc>
    [InstanceOperator]
    public static BooleanInstance operator >=(NumberInstance left, NumberInstance right) => left.Value >= right.Value;

    /// <vl-doc>
    /// <returns>true for number equal to the other number</returns>
    /// </vl-doc>
    [InstanceOperator]
    public static BooleanInstance operator ==(NumberInstance left, NumberInstance right) => left.Value == right.Value;

    /// <vl-doc>
    /// <returns>true for number not equal to the other number</returns>
    /// </vl-doc>
    [InstanceOperator]
    public static BooleanInstance operator !=(NumberInstance left, NumberInstance right) => left.Value != right.Value;
}
