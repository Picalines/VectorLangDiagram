using System;

namespace VectorLang.Model;

/// <vl-library>math constants</vl-library>
internal sealed class MathLibrary : ReflectionLibrary
{
    public static readonly MathLibrary Instance = new();

    private MathLibrary() { }

    /// <vl-doc>
    /// <name>PI</name>
    /// <summary>PI constant (3.1415926535897931)</summary>
    /// </vl-doc>
    [ReflectionConstant("PI")]
    public static readonly NumberInstance PI = Math.PI;

    /// <vl-doc>
    /// <name>E</name>
    /// <summary>E constant (2.7182818284590451)</summary>
    /// </vl-doc>
    [ReflectionConstant("E")]
    public static readonly NumberInstance E = Math.E;

    /// <vl-doc>
    /// <name>Tau</name>
    /// <summary>Tau constant (6.2831853071795862)</summary>
    /// </vl-doc>
    [ReflectionConstant("Tau")]
    public static readonly NumberInstance Tau = Math.Tau;

    /// <vl-doc>
    /// <name>Epsilon</name>
    /// <summary>Epsilon constant - smallest real number bigger than 0</summary>
    /// </vl-doc>
    [ReflectionConstant("Epsilon")]
    public static readonly NumberInstance Epsilon = double.Epsilon;
}
