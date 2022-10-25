namespace VectorLangDocs.Shared.DocumentationModel;

public sealed class InstanceTypeDocumentation : DocumentationItem
{
    public DocumentationRepository<InstanceFieldDocumentation> Fields { get; } = new();

    public DocumentationRepository<InstanceMethodDocumentation> Methods { get; } = new();

    public DocumentationRepository<InstanceUnaryOperatorDocumentation> UnaryOperators { get; } = new();

    public DocumentationRepository<InstanceBinaryOperatorDocumentation> BinaryOperators { get; } = new();

    public InstanceTypeDocumentation(string name) : base(name)
    {
    }
}
