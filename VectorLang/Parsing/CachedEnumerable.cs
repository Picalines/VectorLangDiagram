using System.Collections;
using System.Collections.Generic;

namespace VectorLang.Parsing;

internal sealed class CachedEnumerable<T> : IEnumerable<T>
{
    private readonly List<T> _CachedItems = new();

    private IEnumerator<T>? _Enumerator;

    public CachedEnumerable(IEnumerable<T> enumerable)
    {
        _Enumerator = enumerable.GetEnumerator();
    }

    public IEnumerator<T> GetEnumerator()
    {
        foreach (var cachedItem in _CachedItems)
        {
            yield return cachedItem;
        }

        if (_Enumerator is null)
        {
            yield break;
        }

        while (_Enumerator.MoveNext())
        {
            var current = _Enumerator.Current;

            _CachedItems.Add(current);

            yield return current;
        }

        _Enumerator.Dispose();
        _Enumerator = null;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
