using VectorLang.Tokenization;

namespace VectorLang.Model;

public sealed record Completion(
    CompletionKind Kind,
    string Label,
    string? Detail,
    string Value,
    TextSelection? Scope);
