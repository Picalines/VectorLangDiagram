using System.Collections.Generic;
using System.Linq;
using VectorLang.Compilation;
using VectorLang.Tokenization;

namespace VectorLang.Diagnostics;

internal sealed class CompletionProvider
{
    private static readonly List<Completion> _GlobalScopeCompletions = new()
    {
        Completion.OfKeyword(TokenType.KeywordDef),
        Completion.OfKeyword(TokenType.KeywordConst),
        Completion.OfKeyword(TokenType.KeywordExternal),
    };

    private static readonly List<Completion> _ExpressionScopeKeywordCompletions = new()
    {
        Completion.OfKeyword(TokenType.KeywordVal),
        Completion.OfKeyword(TokenType.KeywordIf),
        Completion.OfKeyword(TokenType.KeywordElse),
    };

    private readonly Stack<CompletionScope> _Scopes = new();

    public void AddDefinitionScope(TextSelection selection, SymbolTable symbols)
    {
        _Scopes.Push(new CompletionScope(selection, CompletionScopeType.Definition, symbols));
    }

    public void AddExpressionScope(TextSelection selection, SymbolTable symbols)
    {
        _Scopes.Push(new CompletionScope(selection, CompletionScopeType.Expression, symbols));
    }

    public IEnumerable<Completion> GetCompletions(TextLocation cursorLocation)
    {
        var currentScope = _Scopes.FirstOrDefault(scope => scope.Selection.Contains(cursorLocation));

        if (currentScope is null)
        {
            foreach (var completion in _GlobalScopeCompletions)
            {
                yield return completion;
            }

            yield break;
        }

        if (currentScope.Type is CompletionScopeType.Expression)
        {
            foreach (var completion in _ExpressionScopeKeywordCompletions)
            {
                yield return completion;
            }
        }

        foreach (var symbol in currentScope.Symbols)
        {
            if (!FilterSymbol(currentScope.Type, symbol))
            {
                continue;
            }

            if (SymbolToCompletion(symbol) is { } completion)
            {
                yield return completion;
            }
        }
    }

    private static bool FilterSymbol(CompletionScopeType scopeType, Symbol symbol) => scopeType switch
    {
        CompletionScopeType.Definition => symbol is InstanceTypeSymbol,

        CompletionScopeType.Expression => symbol is not InstanceTypeSymbol,

        _ => throw new System.NotImplementedException(),
    };

    private static Completion? SymbolToCompletion(Symbol symbol) => symbol switch
    {
        InstanceTypeSymbol { Type: var type } => Completion.OfType(type),

        FunctionSymbol { Function: var function } => Completion.OfFunction(function),

        ExternalValueSymbol { Name: var name, ExternalValue.ValueInstance.Type: var type } => Completion.OfExternalValue(name, type),

        ConstantSymbol { Name: var name, InstanceType: var type } => Completion.OfConstant(name, type),

        VariableSymbol { Name: var name, Type: var type } => Completion.OfVariable(name, type),

        _ => null,
    };
}
