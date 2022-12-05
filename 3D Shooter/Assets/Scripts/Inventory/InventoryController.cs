using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InventoryController
{
    private readonly InventoryModel inventoryM;

    public InventoryModel Model => inventoryM;

    public InventoryController(InventoryModel inventoryM)
    {
        this.inventoryM = inventoryM;
    }

    public void Drop(ItemModel itemM, Transform transform = null)
    {
        itemM.ItemData.DropItem.InstantiateDropItem(itemM, transform);
        inventoryM.RemoveItem(itemM);
    }

    public void RotateItem(ItemModel itemM)
    {
        itemM.Rotate();
    }

    public void SetHandItem(ItemModel itemM)
    {
        Model.HandItemM.PlaceItem(itemM, Vector2Int.zero);
    }

    public ItemModel PickUpItem(IItemContainerModel model, Vector2Int pos)
    {
        var item = model.GetItem(pos);
        if (item == null)
            return null;
        model.RemoveItem(item);
        return item;
    }

    public bool TryPlaceHandItem(IItemContainerModel model, Vector2Int pos)
    {
        var handItemM = Model.HandItemM.GetItem(Vector2Int.zero);
        var bounds = new RectInt(pos, handItemM.Size);

        if (!model.IsInBounds(bounds))
            return false;
        if (model.CanBePlaced(handItemM, pos))
        {
            model.PlaceItem(handItemM, pos);
            model.RemoveItem(handItemM);
            return true;
        }

        var swapItemM = model.GetSwapItem(handItemM, pos);
        if ((swapItemM == null) || swapItemM.ItemData != handItemM.ItemData)
            return false;
        if (swapItemM.ItemData == handItemM.ItemData)
        {
            ItemModel.MoveMaxPossibleAmount(swapItemM, handItemM);
            return false;
        }
        Model.FillUnderfilledItems(swapItemM);
        if (swapItemM.Amount != 0 && inventoryM.CanBeAdded(swapItemM))
        {
            inventoryM.RemoveItem(swapItemM);
            model.PlaceItem(handItemM, pos);
            Model.TryAddItem(swapItemM);
            return true;
        }

        return false;
    }
    public bool MovePossible(ItemModel itemM)
    {
        Model.FillUnderfilledItems(itemM);

        return itemM.Amount == 0 || Model.TryAddItem(itemM);
    }

    public void DropHand()
    {
        Drop(Model.HandItemM.GetItem(Vector2Int.zero));
    }
}
