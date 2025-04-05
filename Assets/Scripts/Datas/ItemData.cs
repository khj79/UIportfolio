using Game.Data.Enums;
using UnityEngine;

[System.Serializable]
public class ItemData : GameData
{
    public string ItemName;
    public ItemCategory Category;
    public ItemRarity Rarity;
    public string IconPath;
    public ItemExtraData ExtraData { get; private set; }

    public void InitializeReferences()
    {
        ExtraData = GetExtraData();
    }

    private ItemExtraData GetExtraData()
    {
        switch (Category)
        {
            case ItemCategory.Equipment:
                return GetItemEquipmentData();
            case ItemCategory.Material:
                return GetItemMaterialData();
            default:
                Debug.LogError($"❌ ItemData Id:{base.Id}의 Category:{Category}에 해당하는 ExtraData가 없습니다.");
                return null;
        }
    }

    private ItemEquipmentData GetItemEquipmentData()
    {
        var equipmentData = DatasheetManager.Instance.GetDataTable<ItemEquipmentData>();

        if (!equipmentData.TryGetValue(base.Id, out var equipment))
        {
            Debug.LogError($"❌ ItemData Id:{base.Id}의 EquipmentData를 찾을 수 없습니다.");
            return null;
        }
        return equipment;
    }

    private ItemMaterialData GetItemMaterialData()
    {
        var materialData = DatasheetManager.Instance.GetDataTable<ItemMaterialData>();

        if (!materialData.TryGetValue(base.Id, out var material))
        {
            Debug.LogError($"❌ ItemData Id:{base.Id}의 MaterialData를 찾을 수 없습니다.");
            return null;
        }
        return material;
    }

    public override bool Validate()
    {
        return base.Validate();
    }
}
