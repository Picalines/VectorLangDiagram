using VectorLang.Model;

namespace VectorLang.Compilation;

internal sealed record InstanceTypeSymbol(InstanceType Type) : Symbol(Type.Name);
