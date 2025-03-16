using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Enums;

[System.Serializable]
public class InventoryItem
{
    public string UniqueId { get; private set; }  
    public int ItemDataId { get; private set; }   
    public int Amount { get; private set; }       
    public List<SealedOptionSlot> SealedOptionSlots { get; private set; }  
    public bool IsEquipment { get; private set; }  

    public InventoryItem(int itemDataId, int amount = 1)
    {
        UniqueId = GenerateUniqueId(); 
        ItemDataId = itemDataId;
        Amount = amount;

        var itemData = DatasheetManager.Instance.GetData<ItemData>(ItemDataId);

        if (itemData?.ExtraData is ItemEquipmentData equipmentData)
        {
            IsEquipment = true;
            InitializeSealedOptionSlots(equipmentData);
            Amount = 1; 
        }
        else if (itemData?.ExtraData is ItemMaterialData materialData)
        {
            IsEquipment = false;
            Amount = Mathf.Min(amount, materialData.MaxStackCount); 
        }
        else
        {
            Debug.LogError($"❌ InventoryItem 생성 실패: ItemDataId({ItemDataId})가 유효하지 않음.");
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

        for (int i = 0; i < normalSlotCount; i++)
            SealedOptionSlots.Add(new SealedOptionSlot(SealedOptionGroupCategory.Normal));

        for (int i = 0; i < uniqueSlotCount; i++)
            SealedOptionSlots.Add(new SealedOptionSlot(SealedOptionGroupCategory.Unique));
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
