using System;
using System.Collections.Generic;
using Game.Enums;

[System.Serializable]
public class ItemEquipmentData
{
    public Int32 Id;
    public string Comment;
    public Int32 Level;
    public EquipmentCategory Category;
    public EquipSlot Slot;
    public Int32 SealedOptionDataId;
    public SealedOptionData SealedOption;
}