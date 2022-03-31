using VectorLang.Model;

namespace VectorLang.Compilation;

internal abstract record CallInstruction(int ArgumentCount) : Instruction;

internal sealed record CallFunctionInstruction(Function Function, int ArgumentCount) : CallInstruction(ArgumentCount);

internal sealed record CallMethodInstruction(InstanceMethod Method, int ArgumentCount) : CallInstruction(ArgumentCount);
