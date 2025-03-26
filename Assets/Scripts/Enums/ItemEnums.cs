using System.Collections.Generic;
using UnityEngine;

namespace Game.Enums
{
    public enum ItemRarity
    {
        Common,
        Uncommon,
        Rare,
        Unique,
        Epic,
    }

    public enum ItemCategory
    {
        Equipment,
        Material
    }

    public enum EquipmentCategory
    {
        Weapon,
        Armor,
        Accessory
    }

    public enum MaterialCategory
    {
        SealedOption
    }

    public enum EquipSlot
    {
        Mainhand,
        Chest,
        Shoulder,
        Legs,
        Waist,
        Feet,
        Neck,
        Wrist,
        Finger
    }

    public enum SealedOptionGroupCategory
    {
        Unique,
        Normal
    }

    public enum ItemCardState
    {
        Selected,
        Unselected
    }

    public enum SealedOptionSlotState
    {
        Empty,
        Normal,
        Silver,
        Gold
    }
}