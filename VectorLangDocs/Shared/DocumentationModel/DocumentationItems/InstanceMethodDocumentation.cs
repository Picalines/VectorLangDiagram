namespace VectorLangDocs.Shared.DocumentationModel;

public sealed class InstanceMethodDocumentation : DocumentationItem, IMemberDocumentationItem, ICallableDocumentationItem
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

    IEnumerable<ParameterDocumentation> ICallableDocumentationItem.Parameters
    {
        get => Parameters.Items;
    }
}
