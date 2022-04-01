using System.Collections.Generic;
using VectorLang.Model;

namespace VectorLang.Compilation;

internal sealed record CompiledExpression(InstanceType Type, IEnumerable<Instruction> Instructions);
