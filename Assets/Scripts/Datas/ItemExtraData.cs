using System;
using Game.Data.Enums;
using UnityEngine;

[System.Serializable]
public class ItemExtraData : GameData
{
    public override bool Validate()
    {
        return base.Validate();
    }
}

[System.Serializable]
public class ItemEquipmentData : ItemExtraData
{
    public Int32 Level;
    public EquipmentCategory Category;
    public EquipSlot Slot;
    public Int32 SealedOptionDataId;
    public SealedOptionData SealedOption { get; private set; }

    private SealedOptionData GetSealedOption()
    {
        if (SealedOptionDataId == 0)
            return null;

        var sealedOptions = DatasheetManager.Instance.GetDataTable<SealedOptionData>();

        if (!sealedOptions.TryGetValue(SealedOptionDataId, out var sealedOption))
        {
            Debug.LogError($"❌ ItemEquipmentData Id:{base.Id}의 SealedOptionDataId:{SealedOptionDataId}가 SealedOptionData 테이블에 없습니다.");
            return null;
        }

        return sealedOption;
    }

    public void InitializeReferences()
    {
        SealedOption = GetSealedOption();
    }

    public override bool Validate()
    {
        return base.Validate();
    }
}

[System.Serializable]
public class ItemMaterialData : ItemExtraData
{
    public MaterialCategory Category;
    public Int32 MaxStackCount;

    public override bool Validate()
    {
        return base.Validate();
    }
}
