using System;
using System.Collections.Generic;
using UnityEngine;

public class SlotModel : IItemContainerModel
{
    public event Action<ItemModel> OnPlaced;
    public event Action OnRemoved;
    public event Action OnChanged;

    public bool DisableEvents = false;
    public readonly ItemType Type;
    private ItemModel itemM;

    public SlotModel(ItemType type)
    {
        Type = type;
    }

    public Vector2Int? GetFreePositon(Vector2Int size, ItemData data) =>
        (itemM != null) || data.Type != Type ? null : Vector2Int.zero;

    public ItemModel GetItem(Vector2Int _) => itemM;

    public bool IsInBounds(RectInt _) => true;

    public void PlaceItem(ItemModel item, Vector2Int _)
    {
        if (itemM != null && item.ItemData.Type != Type)
            throw new InvalidOperationException();
        item.Put(this, Vector2Int.zero);
        itemM = item;

        if (!DisableEvents)
        {
            OnPlaced?.Invoke(item);
            OnChanged?.Invoke();
        }
    }

    public void RemoveItem(ItemModel item)
    {
        if (item != itemM)
            throw new InvalidOperationException();
        itemM.Put(null, Vector2Int.zero);
        itemM = null;

        if (!DisableEvents)
        {
            OnRemoved?.Invoke();
            OnChanged?.Invoke();
        }
    }

    public ItemModel GetSwapItem(ItemModel item, Vector2Int pos) =>
        (item.ItemData.Type == Type) ? itemM : null;

    public bool CanBePlaced(ItemModel item, Vector2Int pos) => 
        (itemM == null) && item.ItemData.Type == Type;

    public IEnumerable<ItemModel> GetUnderfilledItems(ItemData data)
    {
        if (itemM != null && itemM.ItemData == data && !itemM.IsMaxAmount)
            yield return itemM;
    }

    public IEnumerable<ItemModel> GetItems()
    {
        if (itemM != null)
            yield return itemM;
    }
}
