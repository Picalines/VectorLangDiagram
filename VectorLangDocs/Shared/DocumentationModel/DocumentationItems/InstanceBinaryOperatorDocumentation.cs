using VectorLang;
using VectorLang.Model;

namespace VectorLangDocs.Shared.DocumentationModel;

public sealed class InstanceBinaryOperatorDocumentation : DocumentationItem, ICallableDocumentationItem
{
    public BinaryOperator BinaryOperator { get; }

    public InstanceTypeDocumentation RightTypeDocumentation { get; }

    public InstanceTypeDocumentation ReturnTypeDocumentation { get; }

    public string? ReturnValueInfo { get; init; }

    private readonly ParameterDocumentation[] _Parameters;

    public InstanceBinaryOperatorDocumentation(
        BinaryOperator binaryOperator,
        InstanceTypeDocumentation rightTypeDocumentation,
        InstanceTypeDocumentation returnTypeDocumentation) : base(binaryOperator.GetDescription() + " " + rightTypeDocumentation.Name)
    {
        BinaryOperator = binaryOperator;
        RightTypeDocumentation = rightTypeDocumentation;
        ReturnTypeDocumentation = returnTypeDocumentation;

        _Parameters = new[] { new ParameterDocumentation("right", RightTypeDocumentation) };
    }

    IEnumerable<ParameterDocumentation> ICallableDocumentationItem.Parameters
    {
        get => _Parameters;
    }
}
