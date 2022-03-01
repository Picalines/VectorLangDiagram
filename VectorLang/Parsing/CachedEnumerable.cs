using System;
using System.Collections;
using System.Collections.Generic;

namespace VectorLang.Parsing;

internal sealed class CachedEnumerable<T> : IEnumerable<T>, IDisposable
{
    private readonly List<T> _CachedItems = new();

    private IEnumerator<T>? _Enumerator;

    public CachedEnumerable(IEnumerable<T> enumerable)
    {
        _Enumerator = enumerable.GetEnumerator();
    }

    public IEnumerator<T> GetEnumerator()
    {
        if (_Enumerator is null)
        {
            return _CachedItems.GetEnumerator();
        }

        return GetCachingEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Dispose()
    {
        _Enumerator?.Dispose();
        _Enumerator = null;
    }

    private IEnumerator<T> GetCachingEnumerator()
    {
        while (_Enumerator!.MoveNext())
        {
            var current = _Enumerator.Current;

            _CachedItems.Add(current);

            yield return current;
        }

        Dispose();
    }
}
