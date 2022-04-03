using System;
using System.Collections.Generic;
using System.Diagnostics;
using VectorLang.Compilation;
using VectorLang.SyntaxTree;

namespace VectorLang.Model;

// TODO: add compilation and interpretation when these modules are finished

internal sealed class UserFunction : Function
{
    private ValueExpressionNode? _Body;

    private IReadOnlyList<Instruction>? _Instructions = null;

    public UserFunction(string name, CallSignature signature, ValueExpressionNode valueExpression) : base(name, signature)
    {
        _Body = valueExpression;
    }

    private bool IsCompiled => _Instructions is not null;

    public void Compile(SymbolTable programSymbols)
    {
        if (IsCompiled)
        {
            return;
        }

        _Instructions = UserFunctionCompiler.Compile(programSymbols, this, _Body!);
        _Body = null;
    }

    protected override Instance CallInternal(params Instance[] arguments)
    {
        Debug.Assert(IsCompiled);

        throw new NotImplementedException();
    }
}
