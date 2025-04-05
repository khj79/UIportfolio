namespace Game.Data.Enums
{
    public enum CharacterClass
    {
        WeaponMaster,
        SoulBringer,
        Berserker,
        Asura
    }

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

    public enum StatOptionCategory
    {
        PhysicalAttack,
        MagicalAttack,
        Strength,
        Intelligence,
        Constitution,
        Spirit,
        AttackSpeed,
        CastSpeed,
        MoveSpeed,
        MaxHealth,
        MaxMana,
        PhysicalDefence,
        MagicalDefence,
        Hit,
        Dodge,
        PhysicalCritical,
        MagicalCritical,
        EnhanceFire,
        EnhanceWater,
        EnhanceLight,
        EnhanceDark,
        ResistFire,
        ResistWater,
        ResistLight,
        ResistDark,
        DamageMultiplier,
        AdditionalGoldDropRate,
        AdditionalGoodRarityDropRate,
        AdditionalEqupmentDropRate,
        AdditionalMaterialDropRate
    }
}