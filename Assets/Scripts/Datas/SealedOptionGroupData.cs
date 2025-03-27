using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Enums;

[System.Serializable]
public class SealedOptionGroupData : GameData
{
    public SealedOptionGroupCategory Category;
    public List<Int32> SealedOptionEntryIds;
    public List<SealedOptionEntryData> SealedOptionEntries { get; private set; }

    private List<SealedOptionEntryData> GetSealedOptionEntries()
    {
        if (SealedOptionEntryIds == null || SealedOptionEntryIds.Count == 0)
            return null;

        var sealedOptionEntries = DatasheetManager.Instance.GetDataTable<SealedOptionEntryData>();
        var list = new List<SealedOptionEntryData>();

        foreach (var id in SealedOptionEntryIds)
        {
            if (!sealedOptionEntries.TryGetValue(id, out var entry))
            {
                Debug.LogError($"❌ SealedOptionGroupData Id:{base.Id}의 SealedOptionEntryIds 중 id:{id}가 SealedOptionEntryData 테이블에 없습니다.");
                return null;
            }
            list.Add(entry);
        }

        return list;
    }

    public void InitializeReferences()
    {
        SealedOptionEntries = GetSealedOptionEntries();
    }

    public override bool Validate()
    {
        return base.Validate();
    }
}