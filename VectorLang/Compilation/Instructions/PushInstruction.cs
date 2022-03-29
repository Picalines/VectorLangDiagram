namespace VectorLang.Compilation;

internal abstract record PushInstruction : Instruction;

internal sealed record PushNumberInstruction(double Value) : PushInstruction;

internal sealed record PushStringInstruction(string Value) : PushInstruction;
