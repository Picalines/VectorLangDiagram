using VectorLang.Compilation;
using VectorLang.Tokenization;

namespace VectorLang.Diagnostics;

internal enum CompletionScopeType
{
    Definition,
    Expression,
}

internal sealed record CompletionScope(TextSelection Selection, CompletionScopeType Type, SymbolTable Symbols);
