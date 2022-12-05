using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private InventoryHighlighter _inventoryHighlighter;
    [SerializeField] private ItemView _itemVPref;
    [SerializeField] private HandItemView _handItemV;
    [SerializeField] private InventoryCreator _inventoryCreator;
    [SerializeField] private List<SlotView> _slotVs;
    [SerializeField] private List<GridView> _gridVs;
    [SerializeField] private Vector2 _tileSize = new Vector2(32, 32);

    private Dictionary<IItemContainerModel, IItemContainerView> containerVs
      = new Dictionary<IItemContainerModel, IItemContainerView>();
    private bool isOpen = false;

    public InventoryController InventoryC { get => _inventoryCreator.GetController(); }
    private InventoryModel inventoryM { get => _inventoryCreator.GetModel(); }
    public Vector2 TileSize => _tileSize;
    public List<SlotView> SlotVs => _slotVs;
    public List<GridView> GridVs => _gridVs;

    public void ChangeInventoryState()
    {
        isOpen = !isOpen;
        gameObject.SetActive(isOpen);
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        UpdateModel();

        InputHandler.OnLeftMouseUp += LeftMouseButtonUp;
        InputHandler.OnLeftMouseDown += LeftMouseButtonDown;
    }

    private void OnDisable()
    {
        InputHandler.OnLeftMouseUp -= LeftMouseButtonUp;
        InputHandler.OnLeftMouseDown -= LeftMouseButtonDown;
    }

    private void Update()
    {
        if (!isOpen)
            return;
        var updated = false;
        var container = FindContainerView(InputHandler.MousePos);
        if (container is GridView gridV)
        {           
            if (_handItemV.ItemV != null && _handItemV.ItemV.Exists)
            {
                var handItemM = _handItemV.ItemV.Model;
                _inventoryHighlighter.SetGridPosition(handItemM,
                    container.GetGridPosition(InputHandler.MousePos), gridV, _tileSize);
                updated = true;
            }
            else
            {
                var tilePosition = container.GetGridPosition(InputHandler.MousePos);
                var itemM = gridV.GetModel().GetItem(tilePosition);
                if (itemM != null)
                {
                    var itemV = gridV.GetItemView(itemM);
                    if (itemV)
                    {
                        _inventoryHighlighter.SetItem(itemV);
                        updated = true;
                    }
                }
                else
                {
                    _inventoryHighlighter.SetGridCell(gridV.GetGridPosition(InputHandler.MousePos),
                        gridV, _tileSize);
                    updated = true;
                }
            }
        }
        if (container is SlotView slotV)
        {
            _inventoryHighlighter.SetSlotPos(slotV.Rect);
            updated = true;
        }
        if (!updated)
            _inventoryHighlighter.Disable();
    }

    public ItemView InstantiateItemView(ItemModel itemM)
    {
        var itemV = Instantiate(_itemVPref);
        itemV.SetView(this);
        itemV.SetModel(itemM);

        itemM.OnAmountChanged += UpdateView;
        itemM.OnPositionChanged += UpdateView;

        return itemV;
    }

    public void UpdateView()
    {
        var containerMs = inventoryM.GetContainerModels();

        foreach (var model in containerMs)
            containerVs[model].SetModel(model);
        //TODO: Delete unused containers?
    }

    private void UpdateModel()
    {
        var slotMs = inventoryM.SlotMs.ToList();
        var gridMs = inventoryM.GridMs.ToList();

        containerVs.Clear();

        for (int i = 0; i < slotMs.Count; i++)
            containerVs.Add(slotMs[i], _slotVs[i]);
        for (int i = 0; i < gridMs.Count; i++)
            containerVs.Add(gridMs[i], _gridVs[i]);
        containerVs.Add(inventoryM.HandItemM, _handItemV);
        SetModels();
    }

    private void SetModels()
    {
        foreach (var i in containerVs)
        {
            i.Value.SetModel(i.Key);
            i.Key.OnChanged += UpdateView;
        }
    }

    public IEnumerable<IItemContainerView> GetContainerViews() => containerVs.Values;

    public IItemContainerView FindContainerView(Vector2 mousePos)
    {
        foreach (var view in GetContainerViews())
            if (IsInside(mousePos, view.Rect))
                return view;
        return null;
    }

    private bool IsInside(Vector2 mousePos, RectTransform rect)
    {
        var hit = RectTransformUtility
            .ScreenPointToLocalPointInRectangle(rect, mousePos,
            null, out Vector2 local);
        return hit && rect.rect.Contains(local);
    }

    private void LeftMouseButtonDown()
    {
        if (isOpen)
        {
            var activeContainerV = FindContainerView(InputHandler.MousePos);
            if (activeContainerV == null)
                return;
            var tilePosition = activeContainerV.GetGridPosition(InputHandler.MousePos);

            if (!(_handItemV.ItemV != null && _handItemV.ItemV.Exists))
            {
                var handItem = InventoryC.PickUpItem(activeContainerV.GetModel(), tilePosition);
                if (handItem != null)
                    InventoryC.SetHandItem(handItem);
            }
        }
    }

    private void LeftMouseButtonUp()
    {
        if (_handItemV.ItemV && isOpen)
        {
            var activeContainerV = FindContainerView(InputHandler.MousePos);
            if (activeContainerV == null)
            {
                InventoryC.DropHand();
                return;
            }
            var isPlaced = InventoryC.TryPlaceHandItem(activeContainerV.GetModel(),
                                  activeContainerV.GetGridPosition(InputHandler.MousePos));
            if (!isPlaced)
            {
                if (!InventoryC.MovePossible(_handItemV.ItemV.Model))
                    InventoryC.Drop(_handItemV.ItemV.Model);
            }
            else
            {

            }
        }
    }
}
