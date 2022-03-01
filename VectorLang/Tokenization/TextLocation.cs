namespace VectorLang.Tokenization;

public sealed record TextLocation(int Line, int Column)
{
    public TextLocation Shifted(int byLines, int byColumns) => new(Line + byLines, Column + byColumns);

    public override string ToString()
    {
        return $"Line {Line}, Column {Column}";
    }
}
