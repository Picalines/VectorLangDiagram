using System;

namespace VectorLang.Model;

internal sealed class MathLibrary : ReflectionLibrary
{
    public static readonly MathLibrary Instance = new();

    private MathLibrary() { }

    [ReflectionConstant("PI")]
    public static readonly NumberInstance PI = Math.PI;

    [ReflectionConstant("E")]
    public static readonly NumberInstance E = Math.E;

    [ReflectionConstant("Tau")]
    public static readonly NumberInstance Tau = Math.Tau;

    [ReflectionConstant("Epsilon")]
    public static readonly NumberInstance Epsilon = double.Epsilon;
}
