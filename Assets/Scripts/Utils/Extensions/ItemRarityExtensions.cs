using Game.Enums;

public static class ItemRarityExtensions
{
    private static readonly string[] RarityStrings =
    {
        "Common", "Uncommon", "Rare", "Unique", "Epic"
    };

    public static string ToText(this ItemRarity rarity)
    {
        return RarityStrings[(int)rarity];
    }
}
