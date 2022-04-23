using System.Diagnostics;
using VectorLang.Model;
using VectorLang.Tokenization;

namespace VectorLang.Diagnostics;

public sealed class Completion
{
    public CompletionKind Kind { get; }

    public string Label { get; }

    public string? Detail { get; }

    public string Value { get; }

    private Completion(CompletionKind kind, string label, string? detail, string value)
    {
        Kind = kind;
        Label = label;
        Detail = detail;
        Value = value;
    }

    internal static Completion OfKeyword(TokenType keywordType)
    {
        Debug.Assert(keywordType.ToString().Contains("Keyword"));

        var keyword = keywordType.GetDescription().Replace("\'", "");

        return new(CompletionKind.Keyword, keyword, null, keyword);
    }

    internal static Completion OfType(InstanceType type)
    {
        return new(CompletionKind.Type, type.Name, null, type.Name);
    }

    internal static Completion OfField(InstanceType type, string fieldName)
    {
        type.Fields.TryGetValue(fieldName, out var fieldType);

        Debug.Assert(fieldType is not null);

        return new(CompletionKind.Field, fieldName, fieldType.Name, fieldName);
    }

    internal static Completion OfMethod(InstanceType type, string methodName)
    {
        type.Methods.TryGetValue(methodName, out var method);

        Debug.Assert(method is not null);

        return new(CompletionKind.Method, methodName, method.Signature.ReturnType.Name, methodName + "(");
    }

    internal static Completion OfFunction(Function function)
    {
        return new(CompletionKind.Function, function.Name, function.Signature.ReturnType.Name, function.Name + "(");
    }

    internal static Completion OfVariable(string name, InstanceType type)
    {
        return new(CompletionKind.Variable, name, type.Name, name);
    }

    internal static Completion OfConstant(string name, InstanceType type)
    {
        // TODO: value in detail?

        return new(CompletionKind.Constant, name, type.Name, name);
    }
}
