using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Progress;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private InventoryHighlighter _inventoryHighlighter;
    [SerializeField] private ItemView _itemVPref;
    [SerializeField] private HandItemView _handItemV;
    [SerializeField] private InventoryCreator _inventoryCreator;
    [SerializeField] private List<SlotView> _slotVs;
    [SerializeField] private List<GridView> _gridVs;
    [SerializeField] private ItemMenuController _itemMenuController;
    [SerializeField] private Vector2 _tileSize = new Vector2(32, 32);

    private Dictionary<IItemContainerModel, IItemContainerView> containerVs
      = new Dictionary<IItemContainerModel, IItemContainerView>();
    public bool IsOpen = false;

    public InventoryController InventoryC => _inventoryCreator.GetController();
    private InventoryModel inventoryM => _inventoryCreator.GetModel();
    public Vector2 TileSize => _tileSize;
    public List<SlotView> SlotVs => _slotVs;
    public List<GridView> GridVs => _gridVs;

    public void ChangeInventoryState()
    {
        IsOpen = !IsOpen;
        gameObject.SetActive(IsOpen);
        Cursor.lockState = IsOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        UpdateModel();

        InputHandler.LeftMouseUped += OnLeftMouseButtonUp;
        InputHandler.LeftMouseDowned += OnLeftMouseButtonDown;
        InputHandler.RightMouseDowned += OnRightMouseButtonUp;
    }

    private void OnDisable()
    {
        InputHandler.LeftMouseUped -= OnLeftMouseButtonUp;
        InputHandler.LeftMouseDowned -= OnLeftMouseButtonDown;
        InputHandler.RightMouseDowned -= OnRightMouseButtonUp;

        if (_handItemV.ItemV)
        {
            var itemM = inventoryM.HandItemM.GetItem(Vector2Int.zero);
            inventoryM.HandItemM.RemoveItem(itemM);

            if (!inventoryM.AddMaxPossibleAmount(itemM))
                InventoryC.Drop(itemM);
        }
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

    public void UpdateModel()
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
            if (i.Key is SlotExtentionModel slotExtentionM)
                slotExtentionM.OnGridResize += ResizeGrid;
            i.Key.OnChanged += UpdateView;
        }
    }

    private void ResizeGrid(GridModel gridM, Vector2Int size)
    {
        var toDrop = gridM.Resize(size).ToList();
        foreach (var item in toDrop)
        {
            if (inventoryM.AddMaxPossibleAmount(item))
                InventoryC.Drop(item);
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
            .ScreenPointToLocalPointInRectangle(rect, mousePos, null, out Vector2 local);
        return hit && rect.rect.Contains(local);
    }

    private void OnLeftMouseButtonDown()
    {
        if (_itemMenuController.IsOpen && _itemMenuController.IsInBounds(InputHandler.MousePos))
            return;
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

    private void OnLeftMouseButtonUp()
    {
        if (_handItemV.ItemV)
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
                var itemM = inventoryM.HandItemM.GetItem(Vector2Int.zero);
                inventoryM.HandItemM.RemoveItem(itemM);

                if (!inventoryM.AddMaxPossibleAmount(itemM))
                    InventoryC.Drop(itemM);
            }
        }
    }

    private void OnRightMouseButtonUp()
    {
        if (_handItemV.ItemV)
            return;
        var activeContainerV = FindContainerView(InputHandler.MousePos);
        if (activeContainerV == null)
            return;
        var tilePosition = activeContainerV.GetGridPosition(InputHandler.MousePos);
        var itemM = activeContainerV.GetModel().GetItem(tilePosition);
        if (itemM != null)
            _itemMenuController.Open(itemM, InputHandler.MousePos);
    }
}
