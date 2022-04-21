using System;

namespace VectorLang.Model;

internal sealed class MathLibrary : ReflectionLibrary
{
    public static readonly MathLibrary Instance = new();

    private MathLibrary() { }

    [ReflectionConstant("PI")]
    public static readonly NumberInstance PI = new(Math.PI);

    [ReflectionConstant("E")]
    public static readonly NumberInstance E = new(Math.E);

    [ReflectionConstant("Tau")]
    public static readonly NumberInstance Tau = new(Math.Tau);

    [ReflectionConstant("Epsilon")]
    public static readonly NumberInstance Epsilon = new(double.Epsilon);
}
