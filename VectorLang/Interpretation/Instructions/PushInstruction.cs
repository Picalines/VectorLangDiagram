using System;
using VectorLang.Model;

namespace VectorLang.Interpretation;

internal sealed record PushInstruction(Instance Instance) : Instruction;

internal sealed record LazyPushInstruction(Func<Instance> CreateInstance) : Instruction;
