using VectorLang;
using VectorLang.Model;

namespace VectorLangDocs.Shared.DocumentationModel;

internal sealed class InstanceBinaryOperatorDocumentation : DocumentationItem
{
    public BinaryOperator BinaryOperator { get; }

    public InstanceTypeDocumentation RightTypeDocumentation { get; }

    public InstanceTypeDocumentation ReturnTypeDocumentation { get; }

    public string? ReturnValueInfo { get; init; }

    public InstanceBinaryOperatorDocumentation(
        BinaryOperator binaryOperator,
        InstanceTypeDocumentation rightTypeDocumentation,
        InstanceTypeDocumentation returnTypeDocumentation) : base(binaryOperator.GetDescription() + " " + rightTypeDocumentation.Name)
    {
        BinaryOperator = binaryOperator;
        RightTypeDocumentation = rightTypeDocumentation;
        ReturnTypeDocumentation = returnTypeDocumentation;
    }
}
