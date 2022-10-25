using System.Diagnostics.CodeAnalysis;

namespace VectorLangDocs.Shared.DocumentationModel;

public class DocumentationRepository<TItem> where TItem : DocumentationItem
{
    private readonly Dictionary<string, TItem> _Items = new();

    public IEnumerable<TItem> Items
    {
        get => _Items.Values;
    }

    public void Add(TItem item)
    {
        if (_Items.ContainsKey(item.Name))
        {
            throw new ArgumentException($"{typeof(TItem).Name} with name \"{item.Name}\" already exists in the {nameof(DocumentationRepository<TItem>)}", nameof(item));
        }

        _Items.Add(item.Name, item);
    }

    public bool TryGet(string itemName, [NotNullWhen(true)] out TItem? documentationItem)
    {
        return _Items.TryGetValue(itemName, out documentationItem);
    }
}
