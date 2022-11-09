using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InventoryController : MonoBehaviour
{
    [HideInInspector] public InventoryItem HandItem;

    [SerializeField] private List<ItemData> _items;
    [SerializeField] private InventoryView _view;
    [SerializeField] private InventoryModel _model;
    [SerializeField] private GameObject _inventory;

    public InventoryModel Model => _model;

    private bool IsOpen = false;

    private void Awake()
    {
        IsOpen = _inventory.activeSelf;

        InputHandler.OnPressI += ChangeInventoryState;
        InputHandler.OnPressR += RotateItem;
    }

    private void ChangeInventoryState()
    {
        IsOpen = !_inventory.activeSelf;
        _inventory.SetActive(IsOpen);
        Cursor.lockState = IsOpen ? CursorLockMode.Confined : CursorLockMode.Locked;
    }

    private void RotateItem()
    {
        if (!IsOpen && !HandItem)
            return;
        HandItem.Rotate();
    }

    public InventoryItem PickUpItem(IItemContainerModel model, Vector2Int pos)
    {
        var item = model.GetItem(pos);
        if (!item)
            return null;
        model.RemoveItem(item);
        return item;
    }

    private void LeftMouseButtonDown()
    {
        var tilePosition = _view.ViewObj.GetGridPosition(Input.mousePosition);

        if (!HandItem)
            HandItem = PickUpItem(_view.ViewObj.GetModel(), tilePosition);
    }

    private void LeftMouseButtonUp()
    {
        if (HandItem)
            TryPlaceHandItem(_view.ViewObj.GetModel(), _view.GetHandGridPos());
    }

    private void TryPlaceHandItem(IItemContainerModel model, Vector2Int pos)
    {
        var bounds = new RectInt(pos, HandItem.Size);

        if (!model.IsInBounds(bounds))
        {
            HandItem?.GetLastModel.PlaceItem(HandItem, HandItem.LastGridPos);
            HandItem = null;
            return;
        }

        if (model.CanBePlaced(bounds, HandItem))
        {
            model.PlaceItem(HandItem, pos);
            HandItem = null;
            return;
        }

        var swapItem = model.GetSwapItem(bounds, HandItem);
        if (!swapItem || swapItem.ItemData != HandItem.ItemData)
        {
            HandItem?.GetLastModel.PlaceItem(HandItem, HandItem.LastGridPos);
            HandItem = null;
            return;
        }
        if (swapItem.ItemData == HandItem.ItemData)
        {
            var stack = swapItem.FillStack(HandItem.Stack);
            HandItem.SetStack(stack);
            HandItem?.GetLastModel.PlaceItem(HandItem, HandItem.LastGridPos);
            HandItem = null;
            return;
        }
        if ((model as SlotModel) && HandItem.ItemData.Type == swapItem.ItemData.Type
                                 && _model.GetFreePositon(swapItem.ItemData) != null)
        {
            model.RemoveItem(swapItem);
            model.PlaceItem(HandItem, Vector2Int.zero);
            HandItem = null;
            _model.AddItem(swapItem);
        }
    }

    private void DeleteHand()
    {
        HandItem.Delete();
        HandItem = null;
    }
}
