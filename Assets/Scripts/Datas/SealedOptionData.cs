using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SealedOptionData : GameData
{
    public Int64 CostGold;
    public Int32 UniqueCostItemId;
    public Int32 NormalCostItemId;
    public List<Int32> SealedOptionGroupIds;
    public ItemData UniqueCostItem { get; private set; }
    public ItemData NormalCostItem { get; private set; }
    public List<SealedOptionGroupData> SealedOptionGroups { get; private set; }

    public void InitializeReferences()
    {
        
        var sealedOptionGroups = DatasheetManager.Instance.GetDataTable<SealedOptionGroupData>();

        UniqueCostItem = GetUniqueCostItem();
        NormalCostItem = GetNormalCostItem();
        SealedOptionGroups = GetSealedOptionGroups();
    }

    private ItemData GetUniqueCostItem()
    {
        if (UniqueCostItemId == 0)
            return null;
        
        var items = DatasheetManager.Instance.GetDataTable<ItemData>();

        if (!items.TryGetValue(UniqueCostItemId, out var item))
        {
            Debug.LogError($"❌ SealedOptionData Id:{base.Id}의 UniqueCostItemId:{UniqueCostItemId}가 ItemData테이블에 없습니다.");
            return null;    
        }
        return item;
    }

    private ItemData GetNormalCostItem()
    {
        if (NormalCostItemId == 0)
            return null;
        
        var items = DatasheetManager.Instance.GetDataTable<ItemData>();

        if (!items.TryGetValue(NormalCostItemId, out var item))
        {
            Debug.LogError($"❌ SealedOptionData Id:{base.Id}의 NormalCostItemId:{NormalCostItemId}가 ItemData테이블에 없습니다.");
            return null;    
        }
        return item;
    }

    private List<SealedOptionGroupData> GetSealedOptionGroups()
    {
        if (SealedOptionGroupIds == null || SealedOptionGroupIds.Count == 0)
            return null;
        
        var sealedOptionGroups = DatasheetManager.Instance.GetDataTable<SealedOptionGroupData>();
        var list = new List<SealedOptionGroupData>();

        foreach (var id in SealedOptionGroupIds)
        {
            if (!sealedOptionGroups.TryGetValue(id, out var group))
            {
                Debug.LogError($"❌ SealedOptionData Id:{base.Id}의 SealedOptionGroupIds 중 id:{id}가 SealedOptionGroupData테이블에 없습니다.");
                return null;
            }
            list.Add(group);
        }

        return list;
    }
}