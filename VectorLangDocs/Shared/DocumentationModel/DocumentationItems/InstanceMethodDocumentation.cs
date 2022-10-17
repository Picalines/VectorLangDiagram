namespace VectorLangDocs.Shared.DocumentationModel;

internal sealed class InstanceMethodDocumentation : DocumentationItem
{
    public InstanceTypeDocumentation InstanceTypeDocumentation { get; }

    public InstanceTypeDocumentation ReturnTypeDocumentation { get; }

    public DocumentationRepository<ParameterDocumentation> Parameters { get; } = new();

    public string? ReturnValueInfo { get; init; }

    public InstanceMethodDocumentation(
        InstanceTypeDocumentation instanceTypeDocumentation,
        string methodName,
        InstanceTypeDocumentation returnTypeDocumentation) : base(methodName)
    {
        InstanceTypeDocumentation = instanceTypeDocumentation;
        ReturnTypeDocumentation = returnTypeDocumentation;
    }
}
