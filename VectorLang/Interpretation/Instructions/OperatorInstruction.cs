using VectorLang.Model;

namespace VectorLang.Interpretation;

internal abstract record OperatorInstruction : Instruction;

internal sealed record UnaryOperatorInstruction(InstanceUnaryOperator Operator) : OperatorInstruction;

internal sealed record BinaryOperatorInstruction(InstanceBinaryOperator Operator) : OperatorInstruction;
