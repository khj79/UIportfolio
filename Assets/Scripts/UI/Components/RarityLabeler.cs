using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Data.Enums;

public class RarityLabeler : MonoBehaviour
{
    [SerializeField] private ItemRarity rarity;

    [Header("Optional")]
    [SerializeField] private TextMeshProUGUI rarityText;
    [SerializeField] private Image background;

    private void Awake()
    {
        ApplyRarity();
    }

    public void SetRarity(ItemRarity newRarity)
    {
        rarity = newRarity;
        ApplyRarity();
    }

    private void ApplyRarity()
    {
        var info = RarityInfo.Get(rarity);

        if (background != null)
            background.color = info.BgColor;

        if (rarityText != null)
            rarityText.text = info.Text;
    }
}
