using Game.Enums;

[System.Serializable]
public class ItemData : GameData
{
    public string ItemName;
    public ItemCategory Category;
    public ItemRarity Rarity;
    public string IconPath;
    public ItemExtraData ExtraData { get; set; }
}