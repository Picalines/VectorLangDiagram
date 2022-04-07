using System;
using System.Collections.Generic;
using System.Linq;
using VectorLang.Tokenization;

namespace VectorLang.Model;

internal sealed class Reporter
{
    private readonly List<Report> _Reports = new();

    public IReadOnlyList<Report> Reports => _Reports;

    public bool AnyErrors() => Reports.Any(report => report.Severity is Severity.Error);

    public void ReportInfo(TextSelection selection, string message)
    {
        Report(selection, Severity.Info, message);
    }

    public void ReportWarning(TextSelection selection, string message)
    {
        Report(selection, Severity.Warning, message);
    }

    public void ReportError(TextSelection selection, string message)
    {
        Report(selection, Severity.Error, message);
    }

    public bool Catch(TextSelection selection, Action action, Severity severity = Severity.Error)
    {
        try
        {
            action();
        }
        catch (Exception exception)
        {
            Report(selection, severity, exception.Message);
            return true;
        }

        return false;
    }

    private void Report(TextSelection selection, Severity severity, string message)
    {
        _Reports.Add(new Report(selection, severity, message));
    }
}
