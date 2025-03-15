using System;
using System.Collections.Generic;
using Game.Enums;

[System.Serializable]
public class SealedOptionGroupData : GameData
{
    public SealedOptionGroupCategory Category;
    public List<Int32> SealedOptionEntryIds;
    public List<SealedOptionEntryData> SealedOptionEntries;
}