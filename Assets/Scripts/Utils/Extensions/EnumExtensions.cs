using System;
using System.Collections.Generic;

public static class EnumTextExtensions
{
    private static readonly Dictionary<Type, string[]> _enumTextCache = new();

    public static string ToString<TEnum>(this TEnum value) where TEnum : Enum
    {
        var type = typeof(TEnum);

        if (!_enumTextCache.TryGetValue(type, out var names))
        {
            names = Enum.GetNames(type);
            _enumTextCache[type] = names;
        }

        return names[Convert.ToInt32(value)];
    }
}