namespace VectorLangDocs.Shared.DocumentationModel;

public sealed class ConstantDocumentation : DocumentationItem, ILibraryItemDocumentation
{
    public InstanceTypeDocumentation ConstantTypeDocumentation { get; }

    public string? LibraryName { get; init; }

    public ConstantDocumentation(string name, InstanceTypeDocumentation constantTypeDocumentation) : base(name)
    {
        ConstantTypeDocumentation = constantTypeDocumentation;
    }
}
