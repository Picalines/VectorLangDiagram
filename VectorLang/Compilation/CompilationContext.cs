using VectorLang.Diagnostics;

namespace VectorLang.Compilation;

internal sealed class CompilationContext
{
    public Reporter Reporter { get; }

    public CompletionProvider CompletionProvider { get; }

    public SymbolTable Symbols { get; }

    private CompilationContext(Reporter reporter, CompletionProvider completionProvider, SymbolTable symbols)
    {
        Reporter = reporter;
        CompletionProvider = completionProvider;
        Symbols = symbols;
    }

    public CompilationContext() : this(new(), new(), new())
    {
    }

    public CompilationContext WithChildSymbols() => new(Reporter, CompletionProvider, Symbols.Child());

    public void Deconstruct(out Reporter reporter, out CompletionProvider completionProvider, out SymbolTable symbols)
    {
        reporter = Reporter;
        completionProvider = CompletionProvider;
        symbols = Symbols;
    }
}
