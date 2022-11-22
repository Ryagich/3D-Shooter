using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryModel : MonoBehaviour
{
    [SerializeField] private List<SlotModel> _slots;
    [SerializeField] private List<GridModel> _grids;
    [SerializeField] private Transform _parent;

    public List<InventoryItem> items = new List<InventoryItem>();

    private IEnumerable<IItemContainerModel> GetContainerModels()
    {
        foreach (var item in _slots)
            yield return item;
        foreach (var item in _grids)
            yield return item;
    }

    private void RemoveItem(InventoryItem item)
    {
        items.Remove(item);
    }

    private int CanFillOldItem(ItemData data)
    {
        var newData = data;
        var stack = newData.Stack;
        foreach (var item in items)
            if (item.CanBeAddedOnStack(newData))
            {
                var newStack = item.FillStack(stack);
                if (newStack == 0)
                    return 0;
                stack = newStack;
            }
        return stack;
    }

    public void AddItem(ItemData newData)
    {
        var data = newData;
        var stack = CanFillOldItem(data);
        if (stack == 0)
            return;
        var pos = GetFreePositon(data);
        if (pos == null)
            return;
        var item = InstantiateItem(data);
        item.SetStack((int)stack);
        WriteItem(item);
        PlaceItem(item, pos.Value.Item1, pos.Value.Item2, pos.Value.Item3);

        item.OnDelete += RemoveItem;
    }

    public void AddItem(InventoryItem item)
    {
        var stack = CanFillOldItem(item.ItemData);
        item.SetStack(stack);
        if (stack == 0)
            return;
        var pos = GetFreePositon(item.ItemData);
        if (pos == null)
        {
            Destroy(item.gameObject);
            return;
        }
        WriteItem(item);
        PlaceItem(item, pos.Value.Item1, pos.Value.Item2, pos.Value.Item3);

        item.OnDelete += RemoveItem;
    }

    public void PlaceItem(InventoryItem item, Vector2Int pos, IItemContainerModel model, bool rotation)
    {
        if (item.IsRotated != rotation)
            item.Rotate();
        model.PlaceItem(item, pos);
    }

    public InventoryItem InstantiateItem(ItemData data)
    {
        var item = Instantiate(data.ItemPref, _parent);
        item.SetData(data);
        WriteItem(item);
        return item;
    }

    public void WriteItem(InventoryItem item)
    {
        if (!items.Contains(item))
        {
            items.Add(item);
            item.OnDelete += RemoveItem;
        }
    }

    public bool CanBeAdd(ItemData data) => (GetFreePositon(data) != null) || CanFillOldItem(data) == 0;

    public (Vector2Int, IItemContainerModel, bool)? GetFreePositon(ItemData data)
    {
        foreach (var model in GetContainerModels())
        {
            var freePos = model.GetFreePositon(data.Size, data);
            if (freePos != null)
                return (freePos.Value, model, false);
            freePos = model.GetFreePositon(data.RotatedSize, data);
            if (freePos != null)
                return (freePos.Value, model, true);
        }
        return null;
    }
}
