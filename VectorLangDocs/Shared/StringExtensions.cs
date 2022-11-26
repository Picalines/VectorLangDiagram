using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace VectorLangDocs.Shared;

internal static class StringExtensions
{
    private static readonly Regex _IndentRegex = new(@"^\s*", RegexOptions.Compiled, TimeSpan.FromSeconds(0.5));

    private static readonly ConditionalWeakTable<string, string> _DeindentedStrings = new();

    public static string RemoveIndentation(this string str)
    {
        if (_DeindentedStrings.TryGetValue(str, out var cachedResult))
        {
            return cachedResult;
        }

        var lines = str.Trim().Split('\n');
        var indentToRemove = lines.LastOrDefault() is { Length: > 0 } lastLine
            ? _IndentRegex.Match(lastLine).Length
            : 0;

        var linesWithoutIndent = lines
            .Skip(1)
            .Select(line => indentToRemove > line.Length ? "" : line[indentToRemove..]);

        if (lines.Length > 0)
        {
            linesWithoutIndent = linesWithoutIndent.Prepend(lines.First());
        }

        var deindentedStr = string.Join('\n', linesWithoutIndent).Trim();

        _DeindentedStrings.Add(str, deindentedStr);

        return deindentedStr;
    }
}
