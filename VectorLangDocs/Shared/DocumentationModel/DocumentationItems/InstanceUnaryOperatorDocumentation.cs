using VectorLang;
using VectorLang.Model;

namespace VectorLangDocs.Shared.DocumentationModel;

public sealed class InstanceUnaryOperatorDocumentation : DocumentationItem, IMemberDocumentationItem, ICallableDocumentationItem
{
    public UnaryOperator UnaryOperator { get; }

    public InstanceTypeDocumentation InstanceTypeDocumentation { get; }

    public InstanceTypeDocumentation ReturnTypeDocumentation { get; }

    public string? ReturnValueInfo { get; init; }

    public InstanceUnaryOperatorDocumentation(
        UnaryOperator unaryOperator,
        InstanceTypeDocumentation instanceTypeDocumentation,
        InstanceTypeDocumentation returnTypeDocumentation) : base(unaryOperator.GetDescription())
    {
        UnaryOperator = unaryOperator;
        InstanceTypeDocumentation = instanceTypeDocumentation;
        ReturnTypeDocumentation = returnTypeDocumentation;
    }

    IEnumerable<ParameterDocumentation> ICallableDocumentationItem.Parameters { get; } = Array.Empty<ParameterDocumentation>();
}
