using System;
using System.Collections.Generic;
using System.Diagnostics;
using VectorLang.Compilation;
using VectorLang.Interpretation;

namespace VectorLang.Model;

internal sealed class UserFunction : Function
{
    private readonly Lazy<IReadOnlyList<Instruction>> _Instructions;

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

        return Interpreter.Interpret(_Instructions.Value, arguments);
    }
}
