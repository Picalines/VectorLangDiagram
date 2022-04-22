using System.Collections.Generic;
using System.Linq;

namespace VectorLang;

internal static class UtilityExtensions
{
    public static IEnumerable<T> Yield<T>(this T item)
    {
        yield return item;
    }

    public static IEnumerable<T> Counted<T>(this IEnumerable<T> enumerable, out int count)
    {
        var list = enumerable.ToList();

        count = list.Count;

        return list;
    }
}
