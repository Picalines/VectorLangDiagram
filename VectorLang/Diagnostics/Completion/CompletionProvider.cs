using System.Collections.Generic;
using System.Linq;
using VectorLang.Compilation;
using VectorLang.Tokenization;

namespace VectorLang.Diagnostics;

internal sealed class CompletionProvider
{
    private readonly List<Completion> _DefaultCompletions = new()
    {
        Completion.OfKeyword(TokenType.KeywordDef),
        Completion.OfKeyword(TokenType.KeywordConst),
        Completion.OfKeyword(TokenType.KeywordExternal),
    };

    private readonly Stack<CompletionScope> _Scopes = new();

    public void AddScope(TextSelection selection, SymbolTable symbols)
    {
        _Scopes.Push(new CompletionScope(selection, symbols));
    }

    public IReadOnlyList<Completion> GetCompletions(TextLocation cursorLocation)
    {
        var currentScope = _Scopes.FirstOrDefault(scope => scope.Selection.Contains(cursorLocation));

        if (currentScope is null)
        {
            return _DefaultCompletions;
        }

        var completions = new List<Completion>()
        {
            Completion.OfKeyword(TokenType.KeywordVal),
            Completion.OfKeyword(TokenType.KeywordIf),
            Completion.OfKeyword(TokenType.KeywordElse),
        };

        foreach (var symbol in currentScope.Symbols)
        {
            if (SymbolToCompletion(symbol) is { } completion)
            {
                completions.Add(completion);
            }
        }

        return completions;
    }

    private static Completion? SymbolToCompletion(Symbol symbol) => symbol switch
    {
        InstanceTypeSymbol { Type: var type } => Completion.OfType(type),

        FunctionSymbol { Function: var function } => Completion.OfFunction(function),

        ConstantSymbol { Name: var name, InstanceType: var type } => Completion.OfConstant(name, type),

        VariableSymbol { Name: var name, Type: var type } => Completion.OfVariable(name, type),

        _ => null,
    };
}
