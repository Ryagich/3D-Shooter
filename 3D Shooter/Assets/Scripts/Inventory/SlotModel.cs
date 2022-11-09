using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotModel : MonoBehaviour, IItemContainerModel
{
    public event Action<InventoryItem> OnPlaced;
    public event Action OnRemoved;

    [SerializeField] private ItemType _type;
    [SerializeField] private SlotView _view;
    [SerializeField] private Transform _parent;
    private InventoryItem currItem;

    public bool CanBePlaced(RectInt _, InventoryItem item) => !currItem && item.ItemData.Type == _type;
    public bool CanBePlaced(RectInt _, ItemData itemData) => !currItem && itemData.Type == _type;
    public Vector2Int? GetFreePositon(Vector2Int size, ItemData data) => currItem || data.Type != _type ? null : Vector2Int.zero;

    public InventoryItem GetItem(Vector2Int _) => currItem;

    public InventoryItem GetSwapItem(RectInt _, InventoryItem item) => item.ItemData.Type == _type ? currItem : null;

    public IItemContainerView GetView() => _view;

    public bool IsInBounds(RectInt bounds) => true;

    public void PlaceItem(InventoryItem item, Vector2Int _)
    {
        item.Put(this, _view.transform, Vector2Int.zero);
        item.UpdatePositionOnGrid();
        currItem = item;

        OnPlaced?.Invoke(item);
    }

    public void RemoveItem(InventoryItem item)
    {
        if (item == currItem)
            currItem = null;
        item.Put(null, _parent, Vector2Int.zero);

        OnRemoved?.Invoke();
    }
}
