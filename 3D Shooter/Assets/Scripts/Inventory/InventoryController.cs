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
        pos -= handItemM.Size / 2;
        var bounds = new RectInt(pos, handItemM.Size);

        if (!model.IsInBounds(bounds))
            return false;
        if (model.CanBePlaced(handItemM, pos))
        {
            Model.HandItemM.RemoveItem(handItemM);
            model.PlaceItem(handItemM, pos);
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
        if (model is SlotModel slotM)
        {
            slotM.DisableEvents = true;
            var isMoved = MovePossible(swapItemM);
            if (!isMoved)
                Drop(swapItemM);
            slotM.DisableEvents = false;
            Model.HandItemM.RemoveItem(handItemM);
            slotM.PlaceItem(handItemM, Vector2Int.zero);
            return true;
        }
        else
        {
            Model.FillUnderfilledItems(swapItemM);
            if (swapItemM.Amount != 0 && inventoryM.CanBeAdded(swapItemM))
            {
                inventoryM.RemoveItem(swapItemM);
                model.PlaceItem(handItemM, pos);
                Model.TryAddItem(swapItemM);
                return true;
            }
        }

        return false;
    }

    public bool MovePossible(ItemModel itemM)
    {
        Model.FillUnderfilledItems(itemM);
        var containerM = itemM.ContainerM;

        if (itemM.Amount == 0)
            return true;
        if (Model.TryAddItem(itemM))
        {
            if (containerM != null)
                containerM.RemoveItem(itemM);
            return true;
        }
        else
            return false;
    }

    public void DropHand()
    {
        Drop(Model.HandItemM.GetItem(Vector2Int.zero));
    }
}
