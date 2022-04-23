using System.Collections.Generic;
using System.Diagnostics;
using VectorLang.Tokenization;

namespace VectorLang.Model;

internal sealed class CompletionProvider
{
    private readonly List<Completion> _Completions = new();

    private readonly Stack<TextSelection> _Scopes = new();

    public IReadOnlyList<Completion> Completions => _Completions;

    public void BeginScope(TextSelection selection)
    {
        _Scopes.Push(selection);
    }

    public void EndScope()
    {
        _Scopes.Pop();
    }

    public void SuggestKeyword(TokenType keywordType)
    {
        var keyword = keywordType.GetDescription();

        Suggest(CompletionKind.Keyword, keyword, null, keyword);
    }

    public void SuggestType(InstanceType type)
    {
        Suggest(CompletionKind.Type, type.Name, null, type.Name);
    }

    public void SuggestField(InstanceType type, string fieldName)
    {
        type.Fields.TryGetValue(fieldName, out var fieldType);

        Debug.Assert(fieldType is not null);

        Suggest(CompletionKind.Field, fieldName, fieldType.Name, fieldName);
    }

    public void SuggestMethod(InstanceType type, string methodName)
    {
        type.Methods.TryGetValue(methodName, out var method);

        Debug.Assert(method is not null);

        Suggest(CompletionKind.Method, methodName, method.Signature.ReturnType.Name, methodName + "()");
    }

    public void SuggestFunction(Function function)
    {
        Suggest(CompletionKind.Function, function.Name, function.Signature.ReturnType.Name, function.Name + "()");
    }

    public void SuggestVariable(string name, InstanceType type)
    {
        Suggest(CompletionKind.Variable, name, type.Name, name);
    }

    public void SuggestConstant(string name, InstanceType type)
    {
        // TODO: value in detail?

        Suggest(CompletionKind.Constant, name, type.Name, name);
    }

    private void Suggest(CompletionKind kind, string label, string? detail, string value)
    {
        _Scopes.TryPeek(out var scope);

        var completion = new Completion(kind, label, detail, value, scope);

        _Completions.Add(completion);
    }
}
