using UnityEngine;
using Game.Enums;

[System.Serializable]
public class SealedOptionSlot
{
    public SealedOptionGroupCategory Category;  // ✅ Unique or Normal 여부
    public SealedOptionEntryData Entry;  // ✅ 현재 장착된 옵션 (null이면 빈 슬롯)

    public SealedOptionSlot(SealedOptionGroupCategory category)
    {
        Category = category;
        Entry = null;
    }

    public void Equip(SealedOptionEntryData newEntry)
    {
        Entry = newEntry;
    }

    public void Clear()
    {
        Entry = null;
    }

    public bool Validate(string context = "")
    {
        if (Entry == null) return true;

        if (!Entry.Validate())
        {
            Debug.LogWarning($"⚠️ 잘못된 SealedOptionEntryData: {context}, EntryId = {Entry.Id}");
            return false;
        }

        return true;
    }
}
