using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotInventoryExctention : MonoBehaviour
{
    [SerializeField] private InventoryController inventoryC;
    [SerializeField] private InventoryModel inventoryM;
    [SerializeField] private SlotModel slotM;
    [SerializeField] private GridModel GridM;

    private void Awake()
    {
        slotM.OnPlaced += SetGrid;
        slotM.OnRemoved += RemoveGrid;
    }

    private void RemoveGrid()
    {
        var items = (List<InventoryItem>)GridM.Items;
        GridM.Init(Vector2Int.zero);
        foreach (var item in items)
            inventoryM.AddItem(item);
    }

    private void SetGrid(InventoryItem item)
    {
       var newSize = item.GetComponent<ItemInventoryExctention>().Size;
       GridM.Init(newSize);
       GridM.GridView.Init();
    }
}
