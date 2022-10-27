namespace VectorLangDocs.Shared.DocumentationModel;

public interface ICallableDocumentationItem
{
    public InstanceTypeDocumentation ReturnTypeDocumentation { get; }

    public string? ReturnValueInfo { get; }

    public IEnumerable<ParameterDocumentation> Parameters { get; }
}
