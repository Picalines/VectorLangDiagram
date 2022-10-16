using VectorLang;
using VectorLang.Model;

namespace VectorLangDocs.Shared.DocumentationModel;

internal sealed class InstanceBinaryOperatorDocumentation : DocumentationItem
{
    public BinaryOperator BinaryOperator { get; }

    public InstanceBinaryOperatorDocumentation(BinaryOperator binaryOperator) : base(binaryOperator.GetDescription())
    {
        BinaryOperator = binaryOperator;
    }
}
