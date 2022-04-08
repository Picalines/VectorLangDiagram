namespace VectorLang.Model;

public sealed record PlottedVector((double X, double Y) Coordinates,
    string Label,
    (double R, double G, double B) Color);
