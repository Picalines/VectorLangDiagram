using System.Collections.Generic;
using VectorLang.Model;
using VectorLang.Tokenization;

namespace VectorLang.Compilation;

internal sealed record CompiledExpression(InstanceType Type, IEnumerable<Instruction> Instructions)
{
    public CompiledExpression(InstanceType type, Instruction instruction) : this(type, instruction.Yield()) { }

    public void AssertIsAssignableTo(InstanceType expectedType, TextSelection selection)
    {
        ProgramException.MaybeAt(selection, () => Type.AssertIsAssignableTo(expectedType));
    }
}
