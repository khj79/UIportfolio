using System;
using Game.Enums;

[System.Serializable]
public class ItemData
{
    public Int32 Id;
    public string ItemName;
    public ItemCategory Category;
    public ItemRarity Rarity;
    public string IconPath;
    public object ExtraData;
}