using Game.Enums;
using UnityEngine;
using UnityEngine.UI;

public class SealedOptionSlotLabeler : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Sprite transparent;
    [SerializeField] private Sprite normal;
    [SerializeField] private Sprite silver;
    [SerializeField] private Sprite gold;

     public void ApplyState(SealedOptionSlotState state)
    {
        switch (state)
        {
            case SealedOptionSlotState.Empty:
                icon.sprite = transparent;
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
