using VectorLang.Diagnostics;

namespace VectorLang.Compilation;

internal sealed class CompilationContext
{
    public Reporter Reporter { get; }

    public SymbolTable Symbols { get; }

    private CompilationContext(Reporter reporter, SymbolTable symbols)
    {
        Reporter = reporter;
        Symbols = symbols;
    }

    public CompilationContext() : this(new(), new())
    {
    }

    public CompilationContext WithChildSymbols() => new(Reporter, Symbols.Child());
}
