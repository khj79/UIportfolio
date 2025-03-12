using System;
using System.Collections.Generic;
using Game.Enums;

[System.Serializable]
public class SealedOptionGroupData
{
    public Int32 Id;
    public string Comment;
    public SealedOptionGroupCategory Category;
    public List<Int32> SealedOptionEntryIds;
    public List<SealedOptionEntryData> SealedOptionEntries;
}