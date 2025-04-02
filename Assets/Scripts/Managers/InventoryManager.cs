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

    public event Action<InventoryItem> OnItemAdded;
    public event Action<InventoryItem> OnItemRemoved;
    public event Action<List<InventoryItem>> OnInventoryLoaded;
    public event Action<List<InventoryItem>> OnInventoryCleared;

    public void Init()
    {
        LoadInventory();
    }

    public void AddItem(InventoryItem item)
    {
        Items.Add(item);
        Debug.Log($"✅ 아이템 추가됨: {item.ItemDataId}, 현재 개수: {Items.Count}");
        OnItemAdded?.Invoke(item);
    }

    public void RemoveLastItem()
    {
        if (Items.Count == 0) return;
        var removed = Items[^1];
        Items.RemoveAt(Items.Count - 1);
        Debug.Log($"❌ 마지막 아이템 제거됨: {removed.ItemDataId}, 남은 개수: {Items.Count}");
        OnItemRemoved?.Invoke(removed);
    }

    public void Clear()
    {
        Items.Clear();
        Debug.Log("🧹 인벤토리 전체 초기화 완료");
        OnInventoryCleared?.Invoke(Items);
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

        try
        {
            var wrapper = JsonConvert.DeserializeObject<InventoryWrapper>(json);
            var loadedItems = wrapper?.items ?? new List<InventoryItem>();

            Items = new List<InventoryItem>();

            foreach (var item in loadedItems)
            {
                if (item.Validate())
                {
                    Items.Add(item);
                }
            }

            Debug.Log($"📦 인벤토리 로드 완료: {Items.Count}개");
            OnInventoryLoaded?.Invoke(Items);

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
