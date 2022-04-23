using VectorLang.Tokenization;

namespace VectorLang.Model;

public sealed class Report
{
    public TextSelection Selection { get; }

    public ReportSeverity Severity { get; }

    public string Message { get; }

    internal Report(TextSelection selection, ReportSeverity severity, string message)
    {
        Selection = selection;
        Severity = severity;
        Message = message;
    }
}
