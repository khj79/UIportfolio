using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Data.Enums;

[System.Serializable]
public class InventoryItem
{
    public string UniqueId;
    public int ItemDataId;
    public int Amount;
    public List<SealedOptionSlot> SealedOptionSlots;

    public InventoryItem(int itemDataId, int amount = 1)
    {
        UniqueId = GenerateUniqueId(); 
        ItemDataId = itemDataId;
        Amount = amount;

        var itemData = DatasheetManager.Instance.GetData<ItemData>(ItemDataId);

        if (itemData == null)
        {
            Debug.LogError($"❌ InventoryItem 생성 실패: ItemDataId({ItemDataId})가 유효하지 않음.");
            return;
        }

        switch (itemData.Category)
        {
            case ItemCategory.Equipment when itemData.ExtraData is ItemEquipmentData equipmentData:
                InitializeSealedOptionSlots(equipmentData);
                Amount = 1; 
                break;

            case ItemCategory.Material when itemData.ExtraData is ItemMaterialData materialData:
                Amount = Mathf.Min(amount, materialData.MaxStackCount); 
                break;

            default:
                Debug.LogError($"❌ InventoryItem 생성 실패: ItemDataId({ItemDataId})의 카테고리({itemData.Category})가 유효하지 않음.");
                break;
        }
    }

    private string GenerateUniqueId()
    {
        return Guid.NewGuid().ToString(); 
    }

    private void InitializeSealedOptionSlots(ItemEquipmentData equipmentData)
    {
        if (equipmentData.Level < 30)
        {
            SealedOptionSlots = null;
            return;
        }

        SealedOptionSlots = new List<SealedOptionSlot>();
        int normalSlotCount = 0;
        int uniqueSlotCount = 0;

        var itemData = DatasheetManager.Instance.GetData<ItemData>(ItemDataId);
        switch (itemData.Rarity)
        {
            case ItemRarity.Rare:
                normalSlotCount = 1;
                break;
            case ItemRarity.Unique:
                normalSlotCount = 2;
                break;
            case ItemRarity.Epic:
                normalSlotCount = 2;
                uniqueSlotCount = 1;
                break;
        }

        for (int i = 0; i < uniqueSlotCount; i++)
            SealedOptionSlots.Add(new SealedOptionSlot(SealedOptionGroupCategory.Unique));

        for (int i = 0; i < normalSlotCount; i++)
            SealedOptionSlots.Add(new SealedOptionSlot(SealedOptionGroupCategory.Normal));
    }

    public bool Validate()
    {
        var itemData = DatasheetManager.Instance.GetData<ItemData>(ItemDataId);
        bool isValid = true;

        if (itemData == null || !itemData.Validate())
        {
            Debug.LogWarning($"⚠️ 잘못된 ItemData: ItemDataId = {ItemDataId}, UniqueId = {UniqueId}");
            isValid = false;
        }

        if (SealedOptionSlots != null)
        {
            for (int i = 0; i < SealedOptionSlots.Count; i++)
            {
                var slot = SealedOptionSlots[i];
                if (!slot.Validate($"SlotIndex = {i}, Item UniqueId = {UniqueId}"))
                {
                    isValid = false;
                }
            }
        }

        return isValid;
    }

    public ItemData GetItemData()
    {
        return DatasheetManager.Instance.GetData<ItemData>(ItemDataId);
    }

    public void EquipSealedOption(int slotIndex, SealedOptionEntryData option)
    {
        if (SealedOptionSlots == null || slotIndex < 0 || slotIndex >= SealedOptionSlots.Count)
        {
            Debug.LogError($"❌ SealedOption 장착 실패: 잘못된 슬롯 Index({slotIndex})");
            return;
        }

        SealedOptionSlots[slotIndex].Equip(option);
        Debug.Log($"✅ SealedOption 장착 완료: 슬롯 {slotIndex}, 옵션 {option.Id}");
    }
}
