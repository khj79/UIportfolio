using UnityEngine;

public class ColorUtils
{
    public static Color FromHex(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out var color))
            return color;

        Debug.LogWarning($"⚠️ Hex 파싱 실패: {hex}");
        return Color.magenta; // fallback
    }
}
