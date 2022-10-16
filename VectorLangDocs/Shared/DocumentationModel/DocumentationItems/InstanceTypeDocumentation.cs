namespace VectorLangDocs.Shared.DocumentationModel;

internal sealed class InstanceTypeDocumentation : DocumentationItem
{
    public DocumentationRepository<InstanceFieldDocumentation> FieldDocumentations { get; } = new();

    public DocumentationRepository<InstanceMethodDocumentation> MethodDocumentations { get; } = new();

    public DocumentationRepository<InstanceUnaryOperatorDocumentation> UnaryOperatorDocumentations { get; } = new();

    public DocumentationRepository<InstanceBinaryOperatorDocumentation> BinaryOperatorDocumentations { get; } = new();

    public InstanceTypeDocumentation(string name) : base(name)
    {
    }
}
