using System;
using System.Collections.Generic;
using Game.Enums;

[System.Serializable]
public class SealedOptionEntryData : GameData
{
    public Int32 Weight;
    public List<CharacterClass> RecommendedClassesGold;
    public List<CharacterClass> RecommendedClassesSilver;
    public Int32 StatOptionId;
    public StatOptionData StatOption { get; set; }
}