using System.Collections.Generic;
using System.Linq;
using VectorLang.Model;

namespace VectorLang.Compilation;

internal sealed class CompiledExpression
{
    public InstanceType Type { get; }

    public IEnumerable<Instruction> Instructions { get; }

    public bool IsInvalid { get; }

    public static readonly CompiledExpression Invalid = new(InvalidInstanceType.Instance);

    public CompiledExpression(InstanceType type, IEnumerable<Instruction> instructions)
    {
        Type = type;
        Instructions = instructions;

        IsInvalid = Type.IsAssignableTo(InvalidInstanceType.Instance);
    }

    public CompiledExpression(InstanceType type, Instruction instruction) : this(type, instruction.Yield())
    {
    }

    public CompiledExpression(InstanceType type) : this(type, Enumerable.Empty<Instruction>())
    {
    }

    public void Deconstruct(out InstanceType instanceType, out IEnumerable<Instruction> instructions)
    {
        instanceType = Type;
        instructions = Instructions;
    }
}
