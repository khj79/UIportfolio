using System;
using System.Collections.Generic;

[System.Serializable]
public class SealedOptionData
{
    public Int32 Id;
    public string Comment;
    public Int64 CostGold;
    public Int32 UniqueCostItemId;
    public Int32 NormalCostItemId;
    public List<Int32> SealedOptionGroupIds;
    public ItemData UniqueCostItem;
    public ItemData NormalCostItem;
    public List<SealedOptionGroupData> SealedOptionGroups;

}