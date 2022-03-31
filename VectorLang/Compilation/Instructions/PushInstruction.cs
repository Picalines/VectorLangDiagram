using VectorLang.Model;

namespace VectorLang.Compilation;

internal abstract record PushInstruction : Instruction;

internal sealed record PushNumberInstruction(NumberInstance Number) : PushInstruction;

internal sealed record PushStringInstruction(StringInstance String) : PushInstruction;

internal sealed record PushVectorInstruction(VectorInstance Vector) : PushInstruction;

internal sealed record PushColorInstruction(ColorInstance Color) : PushInstruction;
