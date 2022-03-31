using System.Collections.Generic;

namespace VectorLang;

internal static class UtilityExtensions
{
    public static IEnumerable<T> Yield<T>(this T item)
    {
        yield return item;
    }
}
