using System;
using UnityEngine;
using UnityEngine.UI;

public class ScrollSnap : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform contentPanel;
    public int totalPages = 3;
    private float pageWidth;
    private int currentPage;

    void Start()
    {
        pageWidth = contentPanel.rect.width / totalPages;
    }

    public void SnapToNearestPage()
    {
        float horizontalPosition = scrollRect.horizontalNormalizedPosition;
        int targetPage = Mathf.RoundToInt(horizontalPosition * (totalPages - 1));
        scrollRect.horizontalNormalizedPosition = (float)targetPage / (totalPages - 1);
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            SnapToNearestPage();
        }
    }
}
