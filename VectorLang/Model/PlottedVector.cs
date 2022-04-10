namespace VectorLang.Model;

public sealed record PlottedVector(
    (double X, double Y) Start,
    (double X, double Y) End,
    string Label,
    (double R, double G, double B) Color);
