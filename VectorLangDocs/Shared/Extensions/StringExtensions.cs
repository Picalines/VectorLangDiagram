using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace VectorLangDocs.Shared.Extensions;

internal static class StringExtensions
{
    private static readonly Regex _IndentRegex = new(@"^\s*", RegexOptions.Compiled, TimeSpan.FromSeconds(0.5));

    private static readonly ConditionalWeakTable<string, string> _DeindentedStrings = new();

    private static readonly ConditionalWeakTable<string, string> _HtmlIds = new();

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

    public static string ToHtmlId(this string str, string prefixIfStartFromDigit = "n-")
    {
        if (str is "")
        {
            throw new ArgumentException("Can't convert empty string to HTML id", nameof(str));
        }

        if (_HtmlIds.TryGetValue(str, out var cachedResult))
        {
            return cachedResult;
        }

        var builder = new StringBuilder();

        if (char.IsDigit(str[0]))
        {
            builder.Append(prefixIfStartFromDigit);
        }

        var i = 0;
        foreach (var ch in str)
        {
            if (i++ > 0 && builder[^1] is not '-' && (char.IsUpper(ch) || ch is ' '))
            {
                builder.Append('-');
            }

            if (ch is not ' ')
            {
                builder.Append(char.ToLower(ch));
            }
        }

        cachedResult = builder.ToString();

        _HtmlIds.Add(str, cachedResult);
        return cachedResult;
    }
}
