using System.Collections.Generic;

namespace VectorLang.Parsing;

internal static class CachedEnumerableExtensions
{
    public static CachedEnumerable<T> Cached<T>(this IEnumerable<T> enumerable) => new(enumerable);
}
