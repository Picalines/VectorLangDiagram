using System;
using System.ComponentModel;
using System.Reflection;

namespace VectorLang;

internal static class EnumExtensions
{
    public static string? GetDescriptionOrNull(this Enum enumValue)
    {
        return enumValue.GetType()
            .GetField(enumValue.ToString())!
            .GetCustomAttribute<DescriptionAttribute>()?.Description;
    }

    public static string GetDescription(this Enum enumValue)
    {
        return enumValue.GetDescriptionOrNull()
            ?? throw new NotImplementedException($"missing {nameof(DescriptionAttribute)} on enum {enumValue.GetType().Name}.{enumValue}");
    }

    public static string? GetFormattedOrNull(this Enum enumValue, params object?[] formatArgs)
    {
        var formatString = enumValue.GetType()
            .GetField(enumValue.ToString())!
            .GetCustomAttribute<FormatAttribute>()?.Format;

        return formatString is not null ? string.Format(formatString, formatArgs) : null;
    }

    public static string GetFormatted(this Enum enumValue, params object?[] formatArgs)
    {
        return enumValue.GetFormattedOrNull(formatArgs)
            ?? throw new NotImplementedException($"missing {nameof(FormatAttribute)} on enum {enumValue.GetType().Name}.{enumValue}");
    }
}
