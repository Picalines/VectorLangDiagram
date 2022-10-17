using VectorLang;
using VectorLang.Model;

namespace VectorLangDocs.Shared.DocumentationModel;

internal sealed class InstanceUnaryOperatorDocumentation : DocumentationItem
{
    public UnaryOperator UnaryOperator { get; }

    public InstanceTypeDocumentation ReturnTypeDocumentation { get; }

    public string? ReturnValueInfo { get; init; }

    public InstanceUnaryOperatorDocumentation(UnaryOperator unaryOperator, InstanceTypeDocumentation returnTypeDocumentation)
        : base(unaryOperator.GetDescription())
    {
        ReturnTypeDocumentation = returnTypeDocumentation;
    }
}
