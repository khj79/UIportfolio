using System;
using Game.Enums;

[System.Serializable]
public class ItemExtraData : GameData { }

[System.Serializable]
public class ItemEquipmentData : ItemExtraData
{
    public Int32 Level;
    public EquipmentCategory Category;
    public EquipSlot Slot;
    public Int32 SealedOptionDataId;
    public SealedOptionData SealedOption { get; set; }
}

[System.Serializable]
public class ItemMaterialData : ItemExtraData
{
    public MaterialCategory Category;
    public Int32 MaxStackCount;
}
