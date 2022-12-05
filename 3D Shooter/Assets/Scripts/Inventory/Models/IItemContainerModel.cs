using System;
using System.Collections.Generic;
using UnityEngine;

public interface IItemContainerModel
{
    public event Action OnChanged;
    public void PlaceItem(ItemModel item, Vector2Int pos);
    public void RemoveItem(ItemModel item);
    public ItemModel GetSwapItem(ItemModel item, Vector2Int pos);
    public bool CanBePlaced(ItemModel item, Vector2Int pos);
    public ItemModel GetItem(Vector2Int pos);
    public bool IsInBounds(RectInt bounds);
    public Vector2Int? GetFreePositon(Vector2Int size, ItemData data);
    public IEnumerable<ItemModel> GetItems();
    public IEnumerable<ItemModel> GetUnderfilledItems(ItemData data);
}
