using VectorLang;
using VectorLang.Model;

namespace VectorLangDocs.Shared.DocumentationModel;

public sealed class InstanceUnaryOperatorDocumentation : DocumentationItem, ICallableDocumentationItem
{
    public UnaryOperator UnaryOperator { get; }

    public InstanceTypeDocumentation ReturnTypeDocumentation { get; }

    public string? ReturnValueInfo { get; init; }

    public InstanceUnaryOperatorDocumentation(UnaryOperator unaryOperator, InstanceTypeDocumentation returnTypeDocumentation)
        : base(unaryOperator.GetDescription())
    {
        ReturnTypeDocumentation = returnTypeDocumentation;
    }

    IEnumerable<ParameterDocumentation> ICallableDocumentationItem.Parameters { get; } = Array.Empty<ParameterDocumentation>();
}
