using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

public class InventoryManager
{
    public static InventoryManager Instance { get; private set; } = new InventoryManager();

    private const string InventoryKey = "InventoryData";
    public List<InventoryItem> Items { get; private set; } = new List<InventoryItem>();

    public void Init()
    {
        LoadInventory();
    }

    public void AddItem(InventoryItem item)
    {
        Items.Add(item);
        Debug.Log($"✅ 아이템 추가됨: {item.ItemDataId}, 현재 개수: {Items.Count}");
    }

    public void RemoveLastItem()
    {
        if (Items.Count == 0) return;
        var removed = Items[^1];
        Items.RemoveAt(Items.Count - 1);
        Debug.Log($"❌ 마지막 아이템 제거됨: {removed.ItemDataId}, 남은 개수: {Items.Count}");
    }

    public void Clear()
    {
        Items.Clear();
        Debug.Log("🧹 인벤토리 전체 초기화 완료");
    }

    public void SaveInventory()
    {
        var json = JsonConvert.SerializeObject(new InventoryWrapper(Items));
        PlayerPrefs.SetString(InventoryKey, json);
        PlayerPrefs.Save();
    }

    public void LoadInventory()
    {
        if (!PlayerPrefs.HasKey(InventoryKey)) return;

        var json = PlayerPrefs.GetString(InventoryKey);

        // 유효성 검사
        try
        {
            var jObject = JObject.Parse(json);
            var jItems = jObject["items"] as JArray;
            if (jItems == null)
            {
                Debug.LogWarning("❌ 잘못된 인벤토리 형식: items 배열 없음");
                return;
            }

            foreach (var jItem in jItems)
            {
                if (jItem["ItemDataId"] == null || jItem["UniqueId"] == null)
                {
                    Debug.LogWarning("⚠️ 무효한 인벤토리 아이템 감지됨");
                }
            }

            var wrapper = JsonConvert.DeserializeObject<InventoryWrapper>(json);
            Items = wrapper?.items ?? new List<InventoryItem>();

            ValidateGameDataRecursively(Items, "InventoryItem");
        }
        catch (Exception ex)
        {
            Debug.LogError("❌ 인벤토리 로드 실패: " + ex.Message);
        }
    }

    public void AddRandomItem()
    {
        var allItems = DatasheetManager.Instance.GetDataTable<ItemData>()
            .Values.Where(item => item.Category == Game.Enums.ItemCategory.Equipment)
            .ToList();

        if (allItems.Count == 0)
        {
            Debug.LogWarning("⚠️ 장비 아이템이 없습니다.");
            return;
        }

        var randomItem = allItems[UnityEngine.Random.Range(0, allItems.Count)];
        var newItem = new InventoryItem(randomItem.Id);
        AddItem(newItem);
    }

    private void ValidateGameDataRecursively(object obj, string context = "")
    {
        if (obj == null) return;

        var type = obj.GetType();

        // GameData 직접 검사
        if (obj is GameData gd && gd.Id == 0)
        {
            Debug.LogWarning($"⚠️ 잘못된 GameData(Id=0) 객체 발견: {type.Name} {context}");
        }

        // IEnumerable 처리 (List, Array 등)
        if (obj is IEnumerable<object> enumerable)
        {
            int index = 0;
            foreach (var item in enumerable)
            {
                ValidateGameDataRecursively(item, $"{context}[{index}]");
                index++;
            }
            return;
        }

        // 클래스 내부 필드 재귀 검사
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var field in fields)
        {
            var fieldValue = field.GetValue(obj);
            ValidateGameDataRecursively(fieldValue, $"{context}.{field.Name}");
        }
    }


    [Serializable]
    public class InventoryWrapper
    {
        public List<InventoryItem> items;

        public InventoryWrapper(List<InventoryItem> items)
        {
            this.items = items;
        }
    }
}
