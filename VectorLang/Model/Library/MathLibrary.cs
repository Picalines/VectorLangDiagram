using System;

namespace VectorLang.Model;

internal sealed class MathLibrary : ReflectionLibrary
{
    public static readonly MathLibrary Instance = new();

    private MathLibrary() { }

    [ReflectionConstant("PI")]
    public static readonly NumberInstance PI = new(Math.PI);

    [ReflectionConstant("Tau")]
    public static readonly NumberInstance Tau = new(Math.Tau);
}
