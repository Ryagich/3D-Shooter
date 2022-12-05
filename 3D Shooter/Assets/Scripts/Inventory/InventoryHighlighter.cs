using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHighlighter : MonoBehaviour
{
    [SerializeField] private RectTransform hightlighter;

    private Image img;

    private void Awake()
    {
        img = hightlighter.GetComponent<Image>();
    }

    public void SetItem(ItemView item)
    {
        CheckSetParent(item.Rect);
        SetRectSize(item.GetComponent<RectTransform>());
        CheckSetActive(true);
    }

    public void SetGridPosition(ItemModel item, Vector2Int pos, GridView grid, Vector2 tileSize)
    {
        CheckSetParent(grid.Rect);
        hightlighter.sizeDelta = tileSize * item.Size;
        var gridPos = pos - item.Size / 2;
        SetXYPos(grid.CalculatePositionOnGrid(item, gridPos));        
        hightlighter.localRotation = Quaternion.Euler(0, 0, item.IsRotated ? 0.0f : 90.0f);
        CheckSetActive(grid.GetModel().IsInBounds(new RectInt(gridPos, item.Size)));
    }

    public void SetGridCell(Vector2 pos, GridView grid, Vector2 tileSize)
    {
        CheckSetParent(grid.Rect);
        hightlighter.sizeDelta = tileSize;
        SetXYPos((pos * tileSize + tileSize * 0.5f) * new Vector2Int(1, -1));
        hightlighter.localRotation = Quaternion.identity;
        CheckSetActive(true);

        //пасется облако земное 
        //в гуще летних красок
        //ветром пух его трепает
        //солнце шерсть ласкает
    }

    private void SetRectSize(RectTransform rect)
    {
        SetXYPos(Vector2.zero);
        hightlighter.localRotation = Quaternion.identity;
        hightlighter.sizeDelta = rect.sizeDelta;
        CheckSetActive(true);
    }

    public void SetSlotPos(RectTransform rect)
    {
        CheckSetParent(rect);
        SetXYPos(Vector2.zero);
        hightlighter.localRotation = Quaternion.identity;
        hightlighter.sizeDelta = rect.sizeDelta;
        CheckSetActive(true);
    }

    private void SetXYPos(Vector2 pos)
    {
        hightlighter.localPosition = new Vector3(pos.x, pos.y,
            hightlighter.localPosition.z);
    }

    private void CheckSetParent(Transform parent)
    {
        if (hightlighter.parent != parent)
            hightlighter.SetParent(parent);
    }

    private void CheckSetActive(bool active)
    {
        if (img.enabled ^ active)
            img.enabled = active;
    }

    public void Disable()
    {
        CheckSetActive(false);
    }
}
