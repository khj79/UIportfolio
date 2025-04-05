using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Data.Enums;

[System.Serializable]
public class SealedOptionEntryData : GameData
{
    public Int32 Weight;
    public List<CharacterClass> RecommendedClassesGold;
    public List<CharacterClass> RecommendedClassesSilver;
    public Int32 StatOptionId;
    public StatOptionData StatOption { get; private set; }

    private StatOptionData GetStatOption()
    {
        if (StatOptionId == 0)
            return null;
        
        var statOptions = DatasheetManager.Instance.GetDataTable<StatOptionData>();

        if (!statOptions.TryGetValue(StatOptionId, out var statOption))
        {
            Debug.LogError($"❌ SealedOptionEntryData Id:{base.Id}의 StatOptionId:{StatOptionId}가 StatOptionData 테이블에 없습니다.");
            return null;
        }

        return statOption;
    }

    public void InitializeReferences()
    {
        StatOption = GetStatOption();
    }

    public override bool Validate()
    {
        return base.Validate();
    }
}
