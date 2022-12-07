using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GridModel : IItemContainerModel
{
    public event Action OnChanged;

    private ItemModel[,] inventorySlots;
    private Vector2Int size;
    private readonly HashSet<ItemModel> items;

    public Vector2Int Size => size;
    public IEnumerable<ItemModel> GetItems() => items;

    public GridModel(Vector2Int size)
    {
        this.size = size;
        inventorySlots = new ItemModel[this.size.x, this.size.y];
        items = new HashSet<ItemModel>();
    }

    public IEnumerable<ItemModel> Resize(Vector2Int size)
    {
        var newHashSet = new HashSet<ItemModel>();
        this.size = size;

        var toRemove = new List<ItemModel>();
        foreach (var item in GetItems())
            if (IsInBounds(item.GridBounds))
                newHashSet.Add(item);
            else
            {
                toRemove.Add(item);            
                yield return item;
            }
        foreach (var item in toRemove)
            item.Remove();
        items.Clear();
        inventorySlots = new ItemModel[this.size.x, this.size.y];

        foreach (var item in newHashSet)
            PlaceItem(item,item.Position);

        OnChanged?.Invoke();
    }

    public Vector2Int? GetFreePositon(Vector2Int size, ItemData _)
    {
        if (Size == Vector2Int.zero)
            return null;
        var searchSize = this.size - size;

        for (int y = 0; y <= searchSize.y; y++)
            for (int x = 0; x <= searchSize.x; x++)
            {
                var rect = new RectInt(new Vector2Int(x, y), size);
                if (IsInBounds(rect) && IsFree(rect))
                    return new Vector2Int(x, y);
            }

        return null;
    }

    public bool IsFree(RectInt bounds)
    {
        if (!IsInBounds(bounds))
            throw new IndexOutOfRangeException();
        if (Size == Vector2Int.zero)
            return false;
        for (int x = 0; x < bounds.width; x++)
            for (int y = 0; y < bounds.height; y++)
                if (inventorySlots[bounds.xMin + x, bounds.yMin + y] != null)
                    return false;
        return true;
    }

    private IEnumerable<ItemModel> GetItems(RectInt bounds)
    {
        var result = new List<ItemModel>();
        for (int x = 0; x < bounds.width; x++)
            for (int y = 0; y < bounds.height; y++)
            {
                var item = inventorySlots[bounds.xMin + x, bounds.yMin + y];
                if ((item != null) && !result.Contains(item))
                    result.Add(item);
            }
        return result;
    }

    public void PlaceItem(ItemModel item, Vector2Int pos)
    {
        var rect = new RectInt(pos, item.Size);
        if (!IsInBounds(rect) || !IsFree(rect))
            throw new InvalidOperationException();
        item.Put(this, pos);
        FillBounds(item, item.GridBounds);
        items.Add(item);

        OnChanged?.Invoke();
    }

    public void RemoveItem(ItemModel item)
    {
        if (!items.Contains(item))
            throw new InvalidOperationException();
        ClearItemBounds(item);
        item.Put(null, Vector2Int.zero);
        items.Remove(item);

        OnChanged?.Invoke();
    }

    public ItemModel GetSwapItem(ItemModel item, Vector2Int pos)
    {
        var rect = new RectInt(pos, item.Size);
        if (!IsInBounds(rect))
            return null;
        var items = GetItems(rect);
        if (items.Count() != 1)
            return null;
        return items.First();
    }

    public bool CanBePlaced(ItemModel item, Vector2Int pos)
    {
        var rect = new RectInt(pos, item.Size);
        if (!IsInBounds(rect))
            return false;
        return IsFree(rect);
    }

    private void FillBounds(ItemModel item, RectInt bounds)
    {
        for (int x = 0; x < bounds.width; x++)
            for (int y = 0; y < bounds.height; y++)
                inventorySlots[bounds.xMin + x, bounds.yMin + y] = item;
    }

    private void ClearItemBounds(ItemModel item)
    {
        FillBounds(null, item.GridBounds);
    }

    public ItemModel GetItem(Vector2Int pos) =>
           inventorySlots[Mathf.Clamp(pos.x, 0, inventorySlots.GetLength(0) - 1),
                          Mathf.Clamp(pos.y, 0, inventorySlots.GetLength(1) - 1)];

    public bool IsInBounds(Vector2Int pos) => pos.x >= 0 && pos.x < size.x
                                            && pos.y >= 0 && pos.y < size.y;

    public bool IsInBounds(RectInt bounds) =>
                IsInBounds(bounds.min)
             && IsInBounds(bounds.max - Vector2Int.one);

    public IEnumerable<ItemModel> GetUnderfilledItems(ItemData data)
    {
        foreach (var item in items)
            if (item.ItemData == data && !item.IsMaxAmount)
                yield return item;
    }

}
