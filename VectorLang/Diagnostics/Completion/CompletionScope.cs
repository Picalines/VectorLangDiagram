using VectorLang.Compilation;
using VectorLang.Tokenization;

namespace VectorLang.Diagnostics;

internal sealed record CompletionScope(TextSelection Selection, SymbolTable Symbols);
