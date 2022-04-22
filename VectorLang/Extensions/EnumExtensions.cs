using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace VectorLang;

internal static class EnumExtensions
{
    private static readonly Dictionary<Enum, string?> _CachedDescriptions = new();

    private static readonly Dictionary<Enum, string?> _CachedFormats = new();

    public static string? GetDescriptionOrNull(this Enum enumValue)
    {
        if (!_CachedDescriptions.TryGetValue(enumValue, out var description))
        {
            description = enumValue.GetType()
                .GetField(enumValue.ToString())!
                .GetCustomAttribute<DescriptionAttribute>()?.Description;

            _CachedDescriptions.Add(enumValue, description);
        }

        return description;
    }

    public static string GetDescription(this Enum enumValue)
    {
        return enumValue.GetDescriptionOrNull()
            ?? throw new NotImplementedException($"missing {nameof(DescriptionAttribute)} on enum {enumValue.GetType().Name}.{enumValue}");
    }

    public static string? GetFormattedOrNull(this Enum enumValue, params object?[] formatArgs)
    {
        if (!_CachedFormats.TryGetValue(enumValue, out var formatString))
        {
            formatString = enumValue.GetType()
                .GetField(enumValue.ToString())!
                .GetCustomAttribute<FormatAttribute>()?.Format;

            _CachedFormats.Add(enumValue, formatString);
        }

        return formatString is not null ? string.Format(formatString, formatArgs) : null;
    }

    public static string GetFormatted(this Enum enumValue, params object?[] formatArgs)
    {
        return enumValue.GetFormattedOrNull(formatArgs)
            ?? throw new NotImplementedException($"missing {nameof(FormatAttribute)} on enum {enumValue.GetType().Name}.{enumValue}");
    }
}
