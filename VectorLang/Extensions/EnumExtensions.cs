using System;
using System.ComponentModel;
using System.Reflection;

namespace VectorLang;

internal static class EnumExtensions
{
    public static string? GetDescriptionOrNull(this Enum enumValue) =>
        enumValue.GetType()
        .GetField(enumValue.ToString())!
        .GetCustomAttribute<DescriptionAttribute>()?.Description;

    public static string GetDescription(this Enum enumValue) =>
        enumValue.GetDescriptionOrNull()
        ?? throw new NotImplementedException($"missing {nameof(DescriptionAttribute)} on enum {enumValue.GetType().Name}.{enumValue}");
}
