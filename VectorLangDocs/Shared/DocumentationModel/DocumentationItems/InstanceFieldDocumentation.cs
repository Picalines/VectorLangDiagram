namespace VectorLangDocs.Shared.DocumentationModel;

public sealed class InstanceFieldDocumentation : DocumentationItem
{
    public InstanceTypeDocumentation FieldTypeDocumentaion { get; }

    public InstanceFieldDocumentation(string name, InstanceTypeDocumentation fieldTypeDocumentaion) : base(name)
    {
        FieldTypeDocumentaion = fieldTypeDocumentaion;
    }
}
