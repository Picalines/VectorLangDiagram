using System;
using VectorLang.Tokenization;

namespace VectorLang.Model;

public sealed class RuntimeException : Exception
{
    // TODO: call instruction can hold location and assign it in try/catch
    public TextSelection? Selection { get; internal set; } = null;

    private RuntimeException(string message) : base(message) { }

    internal static RuntimeException ZeroDivision() =>
        new("attempt to divide by zero");

    internal static RuntimeException RecursionLimitReached(string functionName, int maxRecusionDepth) =>
        new($"function '{functionName}' has reached the recursion limit ({maxRecusionDepth})");

    internal static RuntimeException PushStackOverflow(int maxStackSize) =>
        new($"push() was called without pop() more than {maxStackSize} times");

    internal static RuntimeException PopBeforePush() =>
        new("pop() was called before push()");

    internal static RuntimeException PlotLimitReached(int maxPlotCount) =>
        new($"plot() was called more than ${maxPlotCount} times");
}
