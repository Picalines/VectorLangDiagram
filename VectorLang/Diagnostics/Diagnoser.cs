using System.Collections.Generic;

namespace VectorLang.Diagnostics;

public sealed class Diagnoser
{
    private Reporter Reporter { get; }

    internal Diagnoser(Reporter reporter)
    {
        Reporter = reporter;
    }

    public IReadOnlyList<Report> Reports => Reporter.Reports;
}
