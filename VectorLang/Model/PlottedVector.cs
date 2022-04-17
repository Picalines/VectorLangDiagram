namespace VectorLang.Model;

public sealed class PlottedVector
{
    public (double X, double Y) Start { get; }

    public (double X, double Y) End { get; }

    public (double R, double G, double B) Color { get; }

    internal PlottedVector(
        VectorInstance start,
        VectorInstance end,
        ColorInstance color)
    {
        Start = start.ToTuple();
        End = end.ToTuple();
        Color = color.ToTuple();
    }
}
