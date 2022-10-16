using VectorLang;
using VectorLang.Model;

namespace VectorLangDocs.Shared.DocumentationModel;

internal sealed class InstanceUnaryOperatorDocumentation : DocumentationItem
{
    public UnaryOperator UnaryOperator { get; }

    public InstanceUnaryOperatorDocumentation(UnaryOperator unaryOperator) : base(unaryOperator.GetDescription())
    {
    }
}
