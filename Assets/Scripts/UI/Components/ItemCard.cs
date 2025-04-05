using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using Game.Enums;

public class ItemCard : MonoBehaviour
{
    [SerializeField] private Image frame;
    [SerializeField] private Image glowFrame;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private RarityLabeler rarityLabeler;
    [SerializeField] private Transform slotContainer;
    [SerializeField] private GameObject slotPrefab; // 꼭 프리팹 연결 필요

    private Vector3 originalScale;
    private Tween scaleTween;
    private Tween glowTween;

    public InventoryItem Item { get; private set; }

    public Action OnClick;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    private SealedOptionSlotState GetSlotState(SealedOptionSlot slot)
    {
        if (slot.Entry == null)
            return SealedOptionSlotState.Empty;

        var entry = slot.Entry;
        var playerClass = GameManager.Instance.CurrentClass;

        if (entry.RecommendedClassesGold != null && entry.RecommendedClassesGold.Contains(playerClass))
            return SealedOptionSlotState.Gold;

        if (entry.RecommendedClassesSilver != null && entry.RecommendedClassesSilver.Contains(playerClass))
            return SealedOptionSlotState.Silver;

        return SealedOptionSlotState.Normal;
    }

    public void Set(InventoryItem item)
    {
        Item = item;

        var data = item.GetItemData();
        iconImage.sprite = LoadIcon(data.IconPath);
        levelText.text = ((data.ExtraData as ItemEquipmentData)?.Level ?? 0).ToString();
        rarityLabeler.SetRarity(data.Rarity);

        // 기존 슬롯 제거
        foreach (Transform child in slotContainer)
        {
            Destroy(child.gameObject);
        }

        // 슬롯 생성
        foreach (var slot in item.SealedOptionSlots)
        {
            var obj = Instantiate(slotPrefab, slotContainer);

            var labeler = obj.GetComponent<SealedOptionSlotLabeler>();
            if (labeler != null)
            {
                var state = GetSlotState(slot);
                labeler.ApplyState(state);
                labeler.ApplyFrameColor(slot.Category);
            }
        }
    }

    public void SetSelected(bool isSelected)
    {
        scaleTween?.Kill();
        glowTween?.Kill();

        if (isSelected)
        {
            scaleTween = transform
                .DOScale(originalScale * 1.05f, 0.15f)
                .SetEase(Ease.OutBack);

            glowTween = glowFrame.DOFade(1.0f, 0.5f);
        }
        else
        {
            scaleTween = transform
                .DOScale(originalScale, 0.15f)
                .SetEase(Ease.OutCubic);

            glowTween = glowFrame.DOFade(0.0f, 0.0f);
        }
    }

    public void OnClickHandler()
    {
        OnClick?.Invoke();
    }

    private Sprite LoadIcon(string path)
    {
        var sprite = Resources.Load<Sprite>(path);
#if UNITY_EDITOR
        if (sprite == null)
        {
            Debug.LogWarning($"❗ Resources.Load 실패: {path}");
        }
#endif
        return sprite;
    }
}
