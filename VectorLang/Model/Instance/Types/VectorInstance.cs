using System;

namespace VectorLang.Model;

/// <vl-doc>
/// <summary>Type that represents a 2D vector</summary>
/// <example>
/// let v = {123, 456};
/// </example>
/// </vl-doc>
[ReflectionInstanceType("vector")]
internal sealed class VectorInstance : ReflectionInstance<VectorInstance>
{
    /// <vl-doc>
    /// <summary>X component of vector</summary>
    /// </vl-doc>
    [InstanceField("x")]
    public NumberInstance X { get; }

    /// <vl-doc>
    /// <summary>Y component of vector</summary>
    /// </vl-doc>
    [InstanceField("y")]
    public NumberInstance Y { get; }

    private readonly Lazy<NumberInstance> _Length;

    public VectorInstance(NumberInstance x, NumberInstance y) : base(InstanceType)
    {
        X = x;
        Y = y;

        _Length = new(() => (X * X + Y * Y).Sqrt());
    }

    public VectorInstance(double x, double y)
        : this(NumberInstance.From(x), NumberInstance.From(y))
    {
    }

    public (double X, double Y) ToTuple() => (X.Value, Y.Value);

    /// <vl-doc>
    /// <returns>length of vector</returns>
    /// </vl-doc>
    [InstanceMethod("length")]
    public NumberInstance Length() => _Length.Value;

    /// <vl-doc>
    /// <returns>vector with same direction but length of 1 (or zero)</returns>
    /// <example>
    /// {5, 0}.normalized() // {1, 0}
    /// {0, 0.1}.normalized() // {0, 1}
    /// {1, 1}.normalized() // {sqrt(2), sqrt(2)}
    /// </example>
    /// </vl-doc>
    [InstanceMethod("normalized")]
    public VectorInstance Normalized() => Length().Value > 0.0 ? new VectorInstance(X / Length(), Y / Length()) : this;

    /// <vl-doc>
    /// <returns>dot product of two vectors (sum of multiplied components)</returns>
    /// </vl-doc>
    [InstanceMethod("dot")]
    public NumberInstance Dot(VectorInstance other) => X * other.X + Y * other.Y;

    /// <vl-doc>
    /// <returns>cosine of angle between two vectors</returns>
    /// </vl-doc>
    [InstanceMethod("angleCos")]
    public NumberInstance AngleCos(VectorInstance other) => Dot(other) / (Length() * other.Length());

    /// <vl-doc>
    /// <returns>angle between two vectors (in radians)</returns>
    /// </vl-doc>
    [InstanceMethod("angle")]
    public NumberInstance Angle(VectorInstance other) => AngleCos(other).Acos();

    /// <vl-doc>
    /// <returns>
    /// new vector with each component lerped to target vector by progress
    /// </returns>
    /// <param name="to">target vector</param>
    /// <param name="progress">lerp parameter (0 - current vector, .. , 1 - target vector)</param>
    /// <example>
    /// {0, 0}.lerp({10, 10}, 0) // {0, 0}
    /// {0, 0}.lerp({10, 10}, 1) // {10, 10}
    /// {0, 0}.lerp({10, 10}, 0.5) // {5, 5}
    /// </example>
    /// </vl-doc>
    [InstanceMethod("lerp")]
    public VectorInstance Lerp(VectorInstance to, NumberInstance progress) => new(X.Lerp(to.X, progress), Y.Lerp(to.Y, progress));

    /// <vl-doc>
    /// <returns>
    /// new vector with length clamped between lower and upper bounds
    /// </returns>
    /// <param name="minLength">lower length bound</param>
    /// <param name="maxLength">upper length bound</param>
    /// </vl-doc>
    [InstanceMethod("clampLength")]
    public VectorInstance ClampLength(NumberInstance minLength, NumberInstance maxLength) => Normalized() * Length().Clamp(minLength, maxLength);

    /// <vl-doc>
    /// <returns>vector rotated counterclockwise by angle in radians</returns>
    /// </vl-doc>
    [InstanceMethod("rotate")]
    public VectorInstance Rotate(NumberInstance angle) => new(
        X * angle.Cos() - Y * angle.Sin(),
        X * angle.Sin() + Y * angle.Cos()
    );

    /// <vl-doc>
    /// <returns>new vector with each component multiplied by components of specified vector</returns>
    /// <example>
    /// {5, 1}.scale({2, 0.5}) // {10, 0.5}
    /// </example>
    /// </vl-doc>
    [InstanceMethod("scale")]
    public VectorInstance Scale(VectorInstance scale) => new(X * scale.X, Y * scale.Y);

    /// <vl-doc>
    /// <returns>the current vector (exists for simmetry)</returns>
    /// </vl-doc>
    [InstanceOperator]
    public static VectorInstance operator +(VectorInstance right) => right;

    /// <vl-doc>
    /// <returns>new vector with each component negated</returns>
    /// </vl-doc>
    [InstanceOperator]
    public static VectorInstance operator -(VectorInstance right) => new(-right.X, -right.Y);

    /// <vl-doc>
    /// <returns>sum of the two vectors</returns>
    /// </vl-doc>
    [InstanceOperator]
    public static VectorInstance operator +(VectorInstance first, VectorInstance second) => new(first.X + second.X, first.Y + second.Y);

    /// <vl-doc>
    /// <returns>difference between the two vectors</returns>
    /// </vl-doc>
    [InstanceOperator]
    public static VectorInstance operator -(VectorInstance first, VectorInstance second) => new(first.X - second.X, first.Y - second.Y);

    /// <vl-doc>
    /// <returns>the dot product of the two vectors</returns>
    /// </vl-doc>
    [InstanceOperator]
    public static NumberInstance operator *(VectorInstance first, VectorInstance second) => first.Dot(second);

    /// <vl-doc>
    /// <returns>vector multiplied by a scalar</returns>
    /// </vl-doc>
    [InstanceOperator]
    public static VectorInstance operator *(VectorInstance vector, NumberInstance number) => new(vector.X * number, vector.Y * number);

    /// <vl-doc>
    /// <returns>vector divided by a scalar. Crashes the program on zero scalar</returns>
    /// </vl-doc>
    [InstanceOperator]
    public static VectorInstance operator /(VectorInstance vector, NumberInstance number)
    {
        if (number.Value == 0.0)
        {
            throw RuntimeException.ZeroDivision();
        }

        return new(vector.X / number, vector.Y / number);
    }

    /// <vl-doc>
    /// <returns>true for two vectors with same components</returns>
    /// </vl-doc>
    [InstanceOperator]
    public static BooleanInstance operator ==(VectorInstance left, VectorInstance right)
    {
        var xEquality = left.X == right.Y;
        var yEquality = left.Y == right.Y;

        return xEquality.Value && yEquality.Value;
    }

    /// <vl-doc>
    /// <returns>true for two vectors with different components</returns>
    /// </vl-doc>
    [InstanceOperator]
    public static BooleanInstance operator !=(VectorInstance left, VectorInstance right)
    {
        return !(left == right);
    }
}
