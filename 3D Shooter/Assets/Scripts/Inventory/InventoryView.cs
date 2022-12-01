using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    public bool IsMouseOverGrid => ViewObj != null;
    public IItemContainerView ViewObj;
    
    [SerializeField] private Transform canvasTransform;
    [SerializeField] private InventoryController controller;
    [SerializeField] private InventoryHighlighter inventoryHighlighter;
    [SerializeField] private string _selectedViewName;

    private void Awake()
    {
        InputHandler.OnMouse += MoveHandItem;
    }   

    private void Update()
    {
        if (ViewObj != null)
            _selectedViewName = ViewObj.GetTransform().gameObject.name;
        else
            _selectedViewName = "null";
        HandleHighlight();
    }

    private void MoveHandItem(Vector3 mouse)
    {
        if (controller.HandItem)
            controller.HandItem.transform.position = mouse;
    }

    private void HandleHighlight()
    {
        var hand = controller.HandItem;
        if (ViewObj == null)
        {
            inventoryHighlighter.Disable();
            return;
        }

        var slotV = (ViewObj as SlotView);
        if (slotV)
        {
            inventoryHighlighter.SetSlotPos(slotV.GetComponent<RectTransform>());
            return;
        }

        if (hand)
        {
            inventoryHighlighter.SetGridPosition(hand, GetHandGridPos(), ViewObj);
            return;
        }

        var item = ViewObj.GetModel().GetItem(ViewObj.GetGridPosition(Input.mousePosition));
        if (item)
            inventoryHighlighter.SetItem(item);
        else
            inventoryHighlighter.Disable();
    }

    public Vector2Int GetHandGridPos()
    {
        var pos = (Vector2)Input.mousePosition;
        pos.x -= (controller.HandItem.Width * 0.5f - 0.5f) * InventoryController.TileSize.x;
        pos.y += (controller.HandItem.Height * 0.5f - 0.5f) * InventoryController.TileSize.y;
        return ViewObj.GetGridPosition(pos);
    }
}
