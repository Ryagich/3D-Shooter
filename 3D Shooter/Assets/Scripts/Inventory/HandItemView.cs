using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandItemView : MonoBehaviour, IItemContainerView
{
    private InventoryController inventoryC;
    private HandItemModel handItemM;

    [SerializeField] private InventoryView _inventoryV;

    public ItemView ItemV { get; private set; }
    public RectTransform Rect { get; private set; }

    public void Awake()
    {
        Rect = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        InputHandler.OnRDown += RotateItem;
    }

    private void OnDisable()
    {
        InputHandler.OnRDown -= RotateItem;
    }

    public IItemContainerModel GetModel() => handItemM;

    private void RotateItem()
    {
        if (handItemM.ItemM != null)
            inventoryC.RotateItem(handItemM.ItemM);
    }

    public void UpdateItem(ItemView item)
    {
        item.Rect.position = new Vector3(InputHandler.MousePos.x,
                                         InputHandler.MousePos.y, item.Rect.position.z);
    }

    public void UpdateView()
    {
        var itemM = handItemM.GetItem(Vector2Int.zero);
        if (ItemV == null && itemM != null)
        {
            ItemV = _inventoryV.InstantiateItemView(itemM);
        }
        if (ItemV)
        {
            UpdateItem(ItemV);
            ItemV.SetModel(itemM);
        }
    }

    public void SetModel(IItemContainerModel containerM)
    {
        if (handItemM != null)
        {
            //TODO: unsub
        }
        handItemM = (HandItemModel)containerM;
        UpdateView();
        //TODO: sub 
    }

    public Vector2Int GetGridPosition(Vector2 mousePos)
    {
        return Vector2Int.zero;
    }
}
