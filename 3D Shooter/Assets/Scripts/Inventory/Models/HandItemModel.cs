using System;
using System.Collections.Generic;
using UnityEngine;

public class HandItemModel : IItemContainerModel
{
    public event Action OnChanged;

    public ItemModel ItemM { get; private set; }

    public bool CanBePlaced(ItemModel item, Vector2Int pos) => ItemM == null;

    public Vector2Int? GetFreePositon(Vector2Int size, ItemData item) =>
        (ItemM == null) ? Vector2Int.zero : null;

    public ItemModel GetItem(Vector2Int pos) => ItemM;

    public IEnumerable<ItemModel> GetItems()
    {
        if (ItemM != null)
            yield return ItemM;
    }

    public ItemModel GetSwapItem(ItemModel item, Vector2Int pos) => ItemM;

    public IEnumerable<ItemModel> GetUnderfilledItems(ItemData data)
    {
        yield break;
    }

    public bool IsInBounds(RectInt bounds) => true;

    public void PlaceItem(ItemModel item, Vector2Int pos)
    {
        if (ItemM != null)
            throw new InvalidOperationException();
        item.Put(this, Vector2Int.zero);
        ItemM = item;

        OnChanged?.Invoke();
    }

    public void RemoveItem(ItemModel item)
    {
        if (item != ItemM)
            throw new InvalidOperationException();
        ItemM.Put(null, Vector2Int.zero);
        ItemM = null;

        OnChanged?.Invoke();
    }
}
