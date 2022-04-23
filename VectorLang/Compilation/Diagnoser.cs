using System.Collections.Generic;
using VectorLang.Model;

namespace VectorLang.Compilation;

public sealed class Diagnoser
{
    public IReadOnlyList<Report> Reports { get; }

    internal Diagnoser(IReadOnlyList<Report> reports)
    {
        Reports = reports;
    }
}
