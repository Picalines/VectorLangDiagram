using System;
using System.Collections.Generic;
using System.Linq;
using VectorLang.Tokenization;

namespace VectorLang.Diagnostics;

internal sealed class Reporter
{
    private readonly List<Report> _Reports = new();

    public IReadOnlyList<Report> Reports => _Reports;

    public bool AnyErrors() => Reports.Any(report => report.Severity is ReportSeverity.Error);

    public void ReportInfo(TextSelection selection, string message)
    {
        Report(selection, ReportSeverity.Info, message);
    }

    public void ReportWarning(TextSelection selection, string message)
    {
        Report(selection, ReportSeverity.Warning, message);
    }

    public void ReportError(TextSelection selection, string message)
    {
        Report(selection, ReportSeverity.Error, message);
    }

    public bool Catch(TextSelection selection, Action action, ReportSeverity severity = ReportSeverity.Error)
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

    private void Report(TextSelection selection, ReportSeverity severity, string message)
    {
        _Reports.Add(new Report(selection, severity, message));
    }
}
