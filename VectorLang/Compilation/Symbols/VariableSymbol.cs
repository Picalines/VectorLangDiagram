using VectorLang.Model;

namespace VectorLang.Compilation;

internal sealed record VariableSymbol(string Name, int Address, InstanceType Type) : Symbol(Name);
