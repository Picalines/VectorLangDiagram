namespace VectorLang.Tokenization;

public sealed record TextLocation(int Line, int Column)
{
    public TextLocation Shifted(int byLines, int byColumns) => new(Line + byLines, Column + byColumns);

    public override string ToString()
    {
        return $"Line {Line}, Column {Column}";
    }

    public static bool operator <=(TextLocation first, TextLocation second)
    {
        if (first.Line == second.Line)
        {
            return first.Column <= second.Column;
        }

        return first.Line < second.Line;
    }

    public static bool operator >=(TextLocation first, TextLocation second)
    {
        if (first.Line == second.Line)
        {
            return first.Column >= second.Column;
        }

        return first.Line > second.Line;
    }

    public static bool operator <(TextLocation first, TextLocation second)
    {
        return !(first == second) && first <= second;
    }

    public static bool operator >(TextLocation first, TextLocation second)
    {
        return !(first == second) && first >= second;
    }
}
