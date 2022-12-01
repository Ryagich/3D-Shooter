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
    [SerializeField] private Vector2 _tileSize = new Vector2(32.0f, 32.0f);

    public static Vector2 TileSize { get; private set; }

    public InventoryModel Model => _model;

    private bool IsOpen = false;

    private void Awake()
    {
        TileSize = _tileSize;
        IsOpen = _inventory.activeSelf;

        InputHandler.OnIDown += ChangeInventoryState;
        InputHandler.OnRDown += RotateItem;
        InputHandler.OnLeftMouseDown += LeftMouseButtonDown;
        InputHandler.OnLeftMouseUp += LeftMouseButtonUp;
    }

    private void ChangeInventoryState()
    {
        if (IsOpen && HandItem)
            LeftMouseButtonUp();
        IsOpen = !_inventory.activeSelf;
        _inventory.SetActive(IsOpen);
        Cursor.lockState = IsOpen ? CursorLockMode.Confined : CursorLockMode.Locked;
        InputHandler.IsInventory = IsOpen;
    }

    private void RotateItem()
    {
        if (!IsOpen || !HandItem)
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
        if (IsOpen)
        {
            if (_view.ViewObj == null)
                return;
            var tilePosition = _view.ViewObj.GetGridPosition(Input.mousePosition);

            if (!HandItem)
                HandItem = PickUpItem(_view.ViewObj.GetModel(), tilePosition);
        }
    }

    private void LeftMouseButtonUp()
    {
        if (HandItem && IsOpen)
        {
            if (_view.ViewObj == null)
                DeleteHand();
            else
                TryPlaceHandItem(_view.ViewObj.GetModel(), _view.GetHandGridPos());
        }
    }

    private void TryPlaceHandItem(IItemContainerModel model, Vector2Int pos)
    {
        var bounds = new RectInt(pos, HandItem.Size);

        if (!model.IsInBounds(bounds))
        {
            CancelDrag();
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
            CancelDrag();
            return;
        }
        if (swapItem.ItemData == HandItem.ItemData)
        {
            var stack = swapItem.FillStack(HandItem.Stack);
            HandItem.SetStack(stack);
            CancelDrag();
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
    private void CancelDrag()
    {
        //HandItem.GetLastModel.PlaceItem(HandItem, HandItem.LastGridPos);
        _model.AddItem(HandItem);
        HandItem = null;
    }

    private void DeleteHand()
    {
        HandItem.Drop(); //hero transform
        Destroy(HandItem.gameObject);
        HandItem = null;
    }
}
