namespace VectorLangDocs.Shared.DocumentationModel;

public sealed class InstanceFieldDocumentation : DocumentationItem, IMemberDocumentationItem
{
    public InstanceTypeDocumentation InstanceTypeDocumentation { get; }

    public InstanceTypeDocumentation FieldTypeDocumentaion { get; }

    public InstanceFieldDocumentation(
        InstanceTypeDocumentation instanceTypeDocumentation,
        string name,
        InstanceTypeDocumentation fieldTypeDocumentaion) : base(name)
    {
        InstanceTypeDocumentation = instanceTypeDocumentation;
        FieldTypeDocumentaion = fieldTypeDocumentaion;
    }
}
