namespace VectorLangDocs.Shared.DocumentationModel;

internal sealed class FunctionDocumentation : DocumentationItem
{
    public InstanceTypeDocumentation ReturnTypeDocumentation { get; }

    public DocumentationRepository<ParameterDocumentation> Parameters { get; } = new();

    public FunctionDocumentation(string funcName, InstanceTypeDocumentation returnTypeDocumentation) : base(funcName)
    {
        ReturnTypeDocumentation = returnTypeDocumentation;
    }
}
