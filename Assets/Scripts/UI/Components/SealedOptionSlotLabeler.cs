using Game.Data.Enums;
using Game.UI.Enums;
using UnityEngine;
using UnityEngine.UI;

public class SealedOptionSlotLabeler : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Image frame;
    [SerializeField] private Sprite normal;
    [SerializeField] private Sprite silver;
    [SerializeField] private Sprite gold;
    [SerializeField] private Sprite defaultFrame;
    [SerializeField] private Sprite yellowFrame;

    public void ApplyFrameColor(SealedOptionGroupCategory category)
    {
        switch (category)
        {
            case SealedOptionGroupCategory.Unique:
                frame.sprite = yellowFrame;
                return;
            default:
                frame.sprite = defaultFrame;
                return;
        }
    }

    public void ApplyState(SealedOptionSlotState state)
    {
        switch (state)
        {
            case SealedOptionSlotState.Empty:
                icon.gameObject.SetActive(false);
                break;

            case SealedOptionSlotState.Normal:
                icon.sprite = normal;
                icon.gameObject.SetActive(true);
                break;

            case SealedOptionSlotState.Silver:
                icon.sprite = silver;
                icon.gameObject.SetActive(true);
                break;

            case SealedOptionSlotState.Gold:
                icon.sprite = gold;
                icon.gameObject.SetActive(true);
                break;

            default:
                Debug.LogWarning($"⚠️ 알 수 없는 SealedOptionVisualType: {state}");
                icon.sprite = null;
                icon.gameObject.SetActive(false);
                break;
        }
    }

    private void Awake()
    {
        ApplyState(SealedOptionSlotState.Empty);
    }
}
