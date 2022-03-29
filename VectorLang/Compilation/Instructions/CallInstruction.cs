namespace VectorLang.Compilation;

internal abstract record CallInstruction(int ArgumentCount) : Instruction;

internal sealed record CallFunctionInstruction(string FunctionName, int ArgumentCount) : CallInstruction(ArgumentCount);

internal sealed record CallMethodInstruction(string MethodName, int ArgumentCount) : CallInstruction(ArgumentCount);
