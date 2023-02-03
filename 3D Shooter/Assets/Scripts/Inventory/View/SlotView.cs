using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotView : MonoBehaviour, IItemContainerView
{
    public event Action<IItemContainerView, Vector2Int> OnMouseUp;
    public event Action<IItemContainerView, Vector2Int> OnMouseDown;

    [SerializeField] private ItemType _type;
    [SerializeField] private InventoryView _inventoryV;

    private SlotModel slotM;
    private ItemView itemV;
    public RectTransform Rect { get; private set; }
    public ItemType Type => _type;
    public IItemContainerModel GetModel() => slotM;

    private void Awake()
    {
        Rect = GetComponent<RectTransform>();
    }

    public void UpdateItem(ItemView item)
    {
        if (!item)
            return;
        if (!Rect)
            Rect = GetComponent<RectTransform>();
        var d = Rect.sizeDelta;
        var itemM = item.Model;
        if ((itemM.ItemData.Size.x > itemM.ItemData.Size.y)
           ^ itemM.IsRotated ^ (d.x > d.y))
            _inventoryV.InventoryC.RotateItem(itemM);
        itemV.Rect.SetParent(transform);
        item.Rect.localPosition = Vector2.zero;
        itemV.UpdateView();
    }

    public void UpdateView()
    {
        var itemM = slotM.GetItem(Vector2Int.zero);

        if (!itemV && itemM != null)
            itemV = _inventoryV.InstantiateItemView(itemM);
        if (itemV != null)
        {
            UpdateItem(itemV);
            itemV.SetModel(itemM);
        }
    }

    public void SetModel(IItemContainerModel containerM)
    {
        if (slotM != null)
        {
            //TODO: unsub
        }
        slotM = (SlotModel)containerM;
        UpdateView();
        //TODO: sub 
    }

    public Vector2Int GetGridPosition(Vector2 mousePos) => Vector2Int.zero;
}
