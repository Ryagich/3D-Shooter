using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemContainerModel
{
    public void PlaceItem(InventoryItem item, Vector2Int pos);
    public void RemoveItem(InventoryItem item);
    public InventoryItem GetSwapItem(RectInt bounds, InventoryItem item);
    public bool CanBePlaced(RectInt bounds, InventoryItem item);
    public InventoryItem GetItem(Vector2Int pos);
    public bool IsInBounds(RectInt bounds);
    public Vector2Int? GetFreePositon(Vector2Int size, ItemData item);
    public int GetFreeStackAmount(ItemData data);
    public IItemContainerView GetView();
}
