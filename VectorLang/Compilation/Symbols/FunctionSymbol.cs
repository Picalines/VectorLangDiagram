using VectorLang.Model;

namespace VectorLang.Compilation;

internal sealed record FunctionSymbol(Function Function) : Symbol(Function.Name);
