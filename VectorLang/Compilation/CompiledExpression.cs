using System.Collections.Generic;
using VectorLang.Model;

namespace VectorLang.Compilation;

internal sealed record CompiledExpression(InstanceType Type, IEnumerable<Instruction> Instructions)
{
    public CompiledExpression(InstanceType type, Instruction instruction) : this(type, instruction.Yield()) { }
}
