namespace VectorLangDocs.Shared.DocumentationModel;

public sealed class FunctionDocumentation : DocumentationItem, ICallableDocumentationItem, ILibraryItemDocumentation
{
    public string? LibraryName { get; init; }

    public InstanceTypeDocumentation ReturnTypeDocumentation { get; }

    public DocumentationRepository<ParameterDocumentation> Parameters { get; } = new();

    public string? ReturnValueInfo { get; init; }

    public FunctionDocumentation(string funcName, InstanceTypeDocumentation returnTypeDocumentation) : base(funcName)
    {
        ReturnTypeDocumentation = returnTypeDocumentation;
    }

    IEnumerable<ParameterDocumentation> ICallableDocumentationItem.Parameters
    {
        get => Parameters.Items;
    }
}
