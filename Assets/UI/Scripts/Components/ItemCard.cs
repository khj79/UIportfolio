using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Game.Enums;

public class ItemCard : MonoBehaviour
{
    [SerializeField] private Image frame;
    [SerializeField] private Image glowFrame;
    private Vector3 originalScale;
    private Tween scaleTween;
    private Tween glowTween;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    private void Update()
    {
        // 테스트: 숫자 1 키 누르면 Selected, 2 누르면 Unselected
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetState(ItemCardState.Selected);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetState(ItemCardState.Unselected);
        }
    }

    public void SetState(ItemCardState state)
    {
        // 기존 트윈 정리
        scaleTween?.Kill();
        glowTween?.Kill();

        switch (state)
        {
            case ItemCardState.Selected:
                scaleTween = transform
                    .DOScale(originalScale * 1.05f, 0.15f)
                    .SetEase(Ease.OutBack);
                glowTween = glowFrame.DOFade(1.0f, 0.5f);
                break;

            case ItemCardState.Unselected:
                scaleTween = transform
                    .DOScale(originalScale, 0.15f)
                    .SetEase(Ease.OutCubic);
                glowTween = glowFrame.DOFade(0.0f, 0.0f);
                break;
        }
    }
}
