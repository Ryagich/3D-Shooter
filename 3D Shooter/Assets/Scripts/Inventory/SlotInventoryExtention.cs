using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotInventoryExtention : MonoBehaviour
{
    [SerializeField] private InventoryController inventoryC;
    [SerializeField] private InventoryModel inventoryM;
    [SerializeField] private SlotModel slotM;
    [SerializeField] private GridModel GridM;

    //private void Awake() //TODO
    //{
    //    slotM.OnPlaced += SetGrid;
    //    slotM.OnRemoved += RemoveGrid;
    //}

    //private void RemoveGrid()
    //{
    //    var items = GridM.Items;
    //    foreach (var item in items)
    //        inventoryM.TryAddItem(item);
    //    GridM.Init(Vector2Int.zero);
    //    GridM.GridView.Init();
    //}

    //private void SetGrid(ItemModel item)
    //{
    //    var newSize = item.GetComponent<ItemInventoryExctention>().Size;
    //    GridM.Init(newSize);
    //    GridM.GridView.Init();
    //}
}
