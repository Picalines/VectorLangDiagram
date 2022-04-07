using VectorLang.Tokenization;

namespace VectorLang.Model;

public enum Severity
{
    Info,
    Warning,
    Error,
}

public record Report(TextSelection Selection, Severity Severity, string Message);
