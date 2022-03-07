using System.Collections.Generic;

namespace VectorLang.SyntaxTree;

internal sealed record Program(IReadOnlyList<Definition> Definitions);
