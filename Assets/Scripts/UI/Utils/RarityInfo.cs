using UnityEngine;
using Game.Data.Enums;
using System.Collections.Generic;

public class RarityInfo
{
    private static readonly Dictionary<ItemRarity, RarityInfo> _map = new()
    {
        { ItemRarity.Common,   new RarityInfo("커먼",   "#FFFFFF") },
        { ItemRarity.Uncommon, new RarityInfo("언커먼", "#68D5ED") },
        { ItemRarity.Rare,     new RarityInfo("레어",   "#B36BFF") },
        { ItemRarity.Unique,   new RarityInfo("유니크", "#FF00FF") },
        { ItemRarity.Epic,     new RarityInfo("에픽",   "#FFB400") },
    };
    
    public string Text { get; private set; }
    public Color Color { get; private set; }

    public RarityInfo(string text, string hexColor)
    {
        Text = text;
        Color = ColorUtils.FromHex(hexColor);
    }

    public static RarityInfo Get(ItemRarity rarity)
    {
        if (_map.TryGetValue(rarity, out var info))
            return info;

        Debug.LogWarning($"⚠️ 알 수 없는 Rarity: {rarity}");
        return new RarityInfo("Invalid", "#FF00FF");
    }
}