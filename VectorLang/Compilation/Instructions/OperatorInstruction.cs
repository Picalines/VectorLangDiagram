using System;
using System.Collections.Generic;
using VectorLang.Model;

namespace VectorLang.Compilation.Instructions;

internal abstract record OperatorInstruction : Instruction;

internal sealed record UnaryOperatorInstruction : OperatorInstruction
{
    public UnaryOperator UnaryOperator { get; }

    private static readonly Dictionary<UnaryOperator, UnaryOperatorInstruction> _Instances = new();

    private UnaryOperatorInstruction(UnaryOperator unaryOperator)
    {
        UnaryOperator = unaryOperator;
    }

    static UnaryOperatorInstruction()
    {
        foreach (var op in Enum.GetValues<UnaryOperator>())
        {
            _Instances[op] = new(op);
        }
    }

    public static UnaryOperatorInstruction From(UnaryOperator unaryOperator)
    {
        return _Instances[unaryOperator];
    }
}

internal sealed record BinaryOperatorInstruction : OperatorInstruction
{
    public BinaryOperator BinaryOperator { get; }

    private static readonly Dictionary<BinaryOperator, BinaryOperatorInstruction> _Instances = new();

    private BinaryOperatorInstruction(BinaryOperator binaryOperator)
    {
        BinaryOperator = binaryOperator;
    }

    static BinaryOperatorInstruction()
    {
        foreach (var op in Enum.GetValues<BinaryOperator>())
        {
            _Instances[op] = new(op);
        }
    }

    public static BinaryOperatorInstruction From(BinaryOperator unaryOperator)
    {
        return _Instances[unaryOperator];
    }
}
