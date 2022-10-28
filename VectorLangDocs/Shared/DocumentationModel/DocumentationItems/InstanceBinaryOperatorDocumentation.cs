using VectorLang;
using VectorLang.Model;

namespace VectorLangDocs.Shared.DocumentationModel;

public sealed class InstanceBinaryOperatorDocumentation : DocumentationItem, IMemberDocumentationItem, ICallableDocumentationItem
{
    public BinaryOperator BinaryOperator { get; }

    public InstanceTypeDocumentation LeftTypeDocumentation { get; }

    public InstanceTypeDocumentation RightTypeDocumentation { get; }

    public InstanceTypeDocumentation ReturnTypeDocumentation { get; }

    public string? ReturnValueInfo { get; init; }

    private readonly ParameterDocumentation[] _Parameters;

    public InstanceBinaryOperatorDocumentation(
        BinaryOperator binaryOperator,
        InstanceTypeDocumentation leftTypeDocumentation,
        InstanceTypeDocumentation rightTypeDocumentation,
        InstanceTypeDocumentation returnTypeDocumentation) : base(binaryOperator.GetDescription() + rightTypeDocumentation.Name)
    {
        BinaryOperator = binaryOperator;
        LeftTypeDocumentation = leftTypeDocumentation;
        RightTypeDocumentation = rightTypeDocumentation;
        ReturnTypeDocumentation = returnTypeDocumentation;

        _Parameters = new[] { new ParameterDocumentation("right", RightTypeDocumentation) };
    }

    InstanceTypeDocumentation IMemberDocumentationItem.InstanceTypeDocumentation
    {
        get => LeftTypeDocumentation;
    }

    IEnumerable<ParameterDocumentation> ICallableDocumentationItem.Parameters
    {
        get => _Parameters;
    }
}
