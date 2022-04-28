using VectorLang.Model;

namespace VectorLang.Compilation;

internal sealed record ExternalValueSymbol(string Name, ExternalValue ExternalValue) : Symbol(Name);
