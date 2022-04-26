using System;
using System.Collections.Generic;
using System.Diagnostics;
using VectorLang.Interpretation;

namespace VectorLang.Model;

internal sealed class UserFunction : Function
{
    private const int MaxRecusionDepth = 1000;

    private readonly Lazy<IReadOnlyList<Instruction>> _Instructions;

    private int _RecusionDepth = 0;

    public UserFunction(string name, CallSignature signature, Lazy<IReadOnlyList<Instruction>> instructions) : base(name, signature)
    {
        _Instructions = instructions;
    }

    private bool IsCompiled => _Instructions.IsValueCreated;

    public void Compile()
    {
        if (IsCompiled)
        {
            return;
        }

        _ = _Instructions.Value;
    }

    protected override Instance CallInternal(params Instance[] arguments)
    {
        Debug.Assert(IsCompiled);

        _RecusionDepth++;

        if (_RecusionDepth > MaxRecusionDepth)
        {
            throw RuntimeException.RecursionLimitReached(Name, MaxRecusionDepth);
        }

        var returned = Interpreter.Interpret(_Instructions.Value, arguments);

        _RecusionDepth = 0;

        return returned;
    }
}
