using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GridView : MonoBehaviour, IItemContainerView
{
    [SerializeField] private InventoryView _inventoryV;
    [SerializeField] private Vector2Int _size;

    private Dictionary<ItemModel, ItemView> itemVs = new Dictionary<ItemModel, ItemView>();
    private GridModel gridM;
    public Vector2Int Size => _size;
    public RectTransform Rect { get; private set; }

    private void Awake()
    {
        Rect = GetComponent<RectTransform>();
    }

    public ItemView GetItemView(ItemModel model) => itemVs.ContainsKey(model) ? itemVs[model] : null;

    public Vector2Int GetGridPosition(Vector2 mousePos)
    {
        var localPos = Rect.InverseTransformPoint(mousePos);
        var gridPos = new Vector2Int((int)(localPos.x / _inventoryV.TileSize.x),
                                    -(int)(localPos.y / _inventoryV.TileSize.y));
        return gridPos;
    }

    public Vector2 CalculatePositionOnGrid(ItemModel itemM, Vector2Int pos)
    => new Vector2((pos.x + itemM.Width / 2f) * _inventoryV.TileSize.x,
                 -(pos.y + itemM.Height / 2f) * _inventoryV.TileSize.y);

    public IItemContainerModel GetModel() => gridM;

    public void UpdateItem(ItemView itemV)
    {
        if (!itemV)
            return;
        SetItemPosition(itemV);
        itemV.UpdateRotation();
    }

    public void SetItemPosition(ItemView itemV)
    {
        itemV.Rect.SetParent(transform);
        itemV.Rect.localPosition = CalculatePositionOnGrid(itemV.Model, itemV.Model.Position);
    }

    public void UpdateView()
    {
        if (!Rect)
            Rect = GetComponent<RectTransform>();
        Rect.sizeDelta = gridM.Size * _inventoryV.TileSize;
        var itemMs = gridM.GetItems();
        foreach (var itemM in itemMs)
        {
            if (!itemVs.ContainsKey(itemM) || !itemVs[itemM])
            {
                var item = _inventoryV.InstantiateItemView(itemM);
                itemVs[itemM] = item;
            }

            var itemV = itemVs[itemM];
            itemV.SetModel(itemM);
            itemV.UpdateView();
            UpdateItem(itemV);
        }
        var toDelete = itemVs.Keys.Except(itemMs).ToList();
        foreach (var itemM in toDelete)
        {
            if (itemVs[itemM])
                Destroy(itemVs[itemM].gameObject);
            itemVs.Remove(itemM);
        }
    }

    public void SetModel(IItemContainerModel containerM)
    {
        gridM = (GridModel)containerM;
        UpdateView();
    }
}