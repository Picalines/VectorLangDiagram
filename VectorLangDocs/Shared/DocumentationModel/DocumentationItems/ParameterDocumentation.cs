namespace VectorLangDocs.Shared.DocumentationModel;

internal sealed class ParameterDocumentation : DocumentationItem
{
    public InstanceTypeDocumentation ParameterTypeDocumentaion { get; }

    public ParameterDocumentation(string name, InstanceTypeDocumentation parameterTypeDocumentaion) : base(name)
    {
        ParameterTypeDocumentaion = parameterTypeDocumentaion;
    }
}
