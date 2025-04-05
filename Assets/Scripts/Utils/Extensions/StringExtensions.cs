using UnityEngine;

public static class StringExtensions
{
    public static string SbAppend(this string baseStr, params object[] parts)
    {
        var sb = new System.Text.StringBuilder(baseStr);
        foreach (var part in parts)
        {
            if (part != null)
                sb.Append(part);
        }
        return sb.ToString();
    }
}
