namespace VectorLangDocs.Shared.DocumentationModel;

internal sealed class ConstantDocumentation : DocumentationItem
{
    public InstanceTypeDocumentation ConstantTypeDocumentation { get; }

    public ConstantDocumentation(string name, InstanceTypeDocumentation constantTypeDocumentation) : base(name)
    {
        ConstantTypeDocumentation = constantTypeDocumentation;
    }
}
