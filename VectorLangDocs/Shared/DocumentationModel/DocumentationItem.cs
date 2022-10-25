namespace VectorLangDocs.Shared.DocumentationModel;

public abstract class DocumentationItem
{
    public string Name { get; }

    public string? Summary { get; init; }

    public string? UsageExample { get; init; }

    public DocumentationItem(string name)
    {
        Name = name;
    }
}
