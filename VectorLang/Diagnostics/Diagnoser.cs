using System.Collections.Generic;
using VectorLang.Tokenization;

namespace VectorLang.Diagnostics;

public sealed class Diagnoser
{
    private readonly Reporter _Reporter;

    private readonly CompletionProvider _CompletionProvider;

    internal Diagnoser(Reporter reporter, CompletionProvider completionProvider)
    {
        _Reporter = reporter;
        _CompletionProvider = completionProvider;
    }

    public IReadOnlyList<Report> Reports => _Reporter.Reports;

    public IReadOnlyList<Completion> GetCompletions(TextLocation cursorLocation) => _CompletionProvider.GetCompletions(cursorLocation);
}
