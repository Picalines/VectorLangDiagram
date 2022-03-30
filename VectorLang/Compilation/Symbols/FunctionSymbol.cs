using VectorLang.Model;

namespace VectorLang.Compilation.Symbols;

internal sealed record FunctionSymbol(Function Function) : Symbol(Function.Name);
