using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private Transform itemCardParent;
    [SerializeField] private GameObject itemCardPrefab;

    private readonly List<InventoryItem> inventoryItems = new();
    private readonly List<ItemCard> itemCards = new();

    private ItemCard selectedCard = null;

    public void AddItem(InventoryItem item)
    {
        inventoryItems.Add(item);

        var go = Instantiate(itemCardPrefab, itemCardParent);
        var card = go.GetComponent<ItemCard>();
        card.Set(item);

        card.OnClick += () => OnCardClicked(card);

        itemCards.Add(card);
    }

    private void OnCardClicked(ItemCard clickedCard)
    {
        if (selectedCard == clickedCard)
            return;

        if (selectedCard != null)
            selectedCard.SetSelected(false);

        selectedCard = clickedCard;
        selectedCard.SetSelected(true);

        Debug.Log($"✅ 선택된 아이템: {selectedCard.Item.ItemDataId}");
    }

    public void ClearAll()
    {
        foreach (var card in itemCards)
            Destroy(card.gameObject);

        itemCards.Clear();
        inventoryItems.Clear();
        selectedCard = null;
    }

    private void Start()
    {
        InventoryManager.Instance.OnInventoryLoaded += OnInventoryLoaded;
        InventoryManager.Instance.OnItemAdded += OnItemAdded;
        InventoryManager.Instance.OnItemRemoved += OnItemRemoved;
    }

    private void OnDestroy()
    {
        InventoryManager.Instance.OnInventoryLoaded -= OnInventoryLoaded;
        InventoryManager.Instance.OnItemAdded -= OnItemAdded;
        InventoryManager.Instance.OnItemRemoved -= OnItemRemoved;
    }

    private void OnItemAdded(InventoryItem item)
    {
        AddItem(item);
    }

    private void OnItemRemoved(InventoryItem item)
    {
        // 맨 마지막 카드 제거
        if (itemCards.Count == 0) return;

        var lastCard = itemCards[^1];
        itemCards.RemoveAt(itemCards.Count - 1);
        Destroy(lastCard.gameObject);
    }

    private void OnInventoryLoaded(List<InventoryItem> items)
    {
        ClearAll(); // 혹시 이전 상태 있을 수도 있으니 초기화
        foreach (var item in items)
        {
            AddItem(item);
        }
    }
}
