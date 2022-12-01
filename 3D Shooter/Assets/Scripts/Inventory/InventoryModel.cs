using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryModel : MonoBehaviour
{
    public static event Action OnUpdateInventory;

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

    public void AddItem(ItemData data)
    {
        var item = InstantiateItem(data);
        AddItem(item);
        OnUpdateInventory?.Invoke();
    }

    public InventoryItem InstantiateAddItem(ItemData data, int stack)
    {
        var item = InstantiateItem(data);
        item.SetStack(stack);
        AddItem(item);
        return item;
    }

    public void AddItem(InventoryItem item)
    {
        FillFreeStacks(item);
        OnUpdateInventory?.Invoke();
        if (!item || item.Stack == 0 || item.IsDestroyed)
            return;
        var pos = GetFreePositon(item.ItemData);
        if (pos == null)
        {
            item.Drop();
            Destroy(item.gameObject);
            OnUpdateInventory?.Invoke();
            return;
        }
        RegisterItem(item);
        PlaceItem(item, pos.Value.Item1, pos.Value.Item2, pos.Value.Item3);
        OnUpdateInventory?.Invoke();
    }

    public void PlaceItem(InventoryItem item, Vector2Int pos, IItemContainerModel model, bool rotation)
    {
        if (item.IsRotated != rotation)
            item.Rotate();
        model.PlaceItem(item, pos);
    }

    public InventoryItem InstantiateItem(ItemData data)
    {
        var item = Instantiate(data.ItemPref, null);
        item.SetData(data);
        RegisterItem(item); 
        return item;
    }


    public void RegisterItem(InventoryItem item)
    {
        if (!items.Contains(item))
        {
            items.Add(item);
            item.OnDelete += RemoveItem;
        }
    }

    private void RemoveItem(InventoryItem item)
    {
        items.Remove(item);
        OnUpdateInventory.Invoke();
    }

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

    public int GetFreeStackAmount(ItemData data)
    {
        //return GetContainerModels().Select(i => i.GetFreeStackAmount(data)).Sum();
        return items.Select(i => i.ItemData == data ? i.FreeAmount : 0).Sum();
    }

    public void FillFreeStacks(InventoryItem item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].ItemData == item)
            {
                item.SetStack(items[i].FillStack(item.Stack));
                if (!item || item.Stack == 0)
                    return;
            }
        }
    }
}
