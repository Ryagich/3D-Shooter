using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHighlighter : MonoBehaviour
{
    [SerializeField] private RectTransform hightlighter;

    public ItemView item;
    public IItemContainerView grid;

    public void SetItem(ItemView item)
    {
        if (this.item == item)
            return;
        this.item = item;
        hightlighter.SetParent(item.transform);
        SetRectSize(item.GetComponent<RectTransform>());
        hightlighter.gameObject.SetActive(true);
    }

    public void SetGridPosition(ItemView item, Vector2Int pos, GridView grid)
    {
        hightlighter.SetParent(grid.Rect);
        SetRectSize(item.GetComponent<RectTransform>());
        hightlighter.localPosition = grid.CalculatePositionOnGrid(item, pos);
        hightlighter.localRotation = item.transform.localRotation;
        hightlighter.gameObject.SetActive(true);
    }

    private void SetRectSize(RectTransform rect)
    {
        hightlighter.localPosition = Vector2.zero;
        hightlighter.localRotation = Quaternion.identity;
        hightlighter.sizeDelta = rect.sizeDelta;
    }

    public void SetSlotPos(RectTransform rect)
    {
        hightlighter.SetParent(rect);
        hightlighter.localPosition = Vector2.zero;
        hightlighter.localRotation = Quaternion.identity;
        hightlighter.sizeDelta = rect.sizeDelta;
        hightlighter.gameObject.SetActive(true);
    }

    public void Disable()
    {
        item = null;
        grid = null;
        hightlighter.gameObject.SetActive(false);
    }
}
