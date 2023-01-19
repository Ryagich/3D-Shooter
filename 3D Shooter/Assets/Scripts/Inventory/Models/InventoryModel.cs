using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryModel
{
    public event Action OnInventoryChange;

    public readonly HandItemModel HandItemM;

    private readonly List<SlotModel> slotMs;
    private readonly List<GridModel> gridMs;

    public IEnumerable<SlotModel> SlotMs => slotMs;
    public IEnumerable<GridModel> GridMs => gridMs;

    public InventoryModel(IEnumerable<SlotModel> slots, IEnumerable<GridModel> grids)
    {
        slotMs = new List<SlotModel>(slots);
        gridMs = new List<GridModel>(grids);
        HandItemM = new HandItemModel();
    }

    public IEnumerable<ItemModel> GetUnderfilledItems(ItemData data)
    {
        foreach (var model in GetContainerModels())
            foreach (var item in model.GetUnderfilledItems(data))
                yield return item;
    }

    public void RemoveItem(ItemModel itemM)
    {
        foreach (var model in GetContainerModels())
            if (model.GetItems().Contains(itemM))
            {
                model.RemoveItem(itemM);
                OnInventoryChange?.Invoke();
                return;
            }
        throw new ArgumentException();
    }

    public IEnumerable<ItemModel> GetItems()
    {
        foreach (var model in GetContainerModels())
            foreach (var item in model.GetItems())
                yield return item;
    }

    public IEnumerable<IItemContainerModel> GetContainerModels()
    {
        foreach (var slotM in slotMs)
            yield return slotM;
        foreach (var gridM in gridMs)
            yield return gridM;
        yield return HandItemM;
    }

    public bool CanBeAdded(ItemModel item) => GetFreePositon(item.ItemData) != null;
    
    public bool TryAddItem(ItemModel item)
    {
        var pos = GetFreePositon(item.ItemData);
        if (pos == null)
            return false;
        PlaceItem(item, pos);
        OnInventoryChange?.Invoke();
        return true;
    }

    public void PlaceItem(ItemModel item, ItemPosition pos)
    {
        if (item.IsRotated != pos.IsRotated)
            item.Rotate();
        pos.Model.PlaceItem(item, pos.Position);
    }

    public ItemPosition GetFreePositon(ItemData data)
    {
        foreach (var model in GetContainerModels())
        {
            var freePos = model.GetFreePositon(data.Size, data);
            if (freePos != null)
                return new ItemPosition(model, freePos.Value, false);
            freePos = model.GetFreePositon(data.RotatedSize, data);
            if (freePos != null)
                return new ItemPosition(model, freePos.Value, true);
        }
        return null;
    }

    public int FillUnderfilledItems(ItemModel from)
    {
        var moveAmount = 0;
        foreach (var item in GetUnderfilledItems(from.ItemData))
        {
            if (item == from)
                continue;
            moveAmount += ItemModel.MoveMaxPossibleAmount(from, item);
            if (from.Amount == 0)
                break;
                //return moveAmount;
        }
        OnInventoryChange?.Invoke();
        return moveAmount;
    }
}