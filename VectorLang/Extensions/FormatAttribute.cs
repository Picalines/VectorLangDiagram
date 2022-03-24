using System;

namespace VectorLang;

[AttributeUsage(AttributeTargets.Field)]
internal sealed class FormatAttribute : Attribute
{
    public string Format { get; }

    public FormatAttribute(string format)
    {
        Format = format;
    }
}
