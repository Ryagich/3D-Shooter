using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryCreator : MonoBehaviour
{
    [SerializeField] private InventoryView inventoryV;
    [SerializeField] private HandItemView _handItem;

    private InventoryModel inventoryM;
    private InventoryController inventoryC;

    public InventoryModel GetModel()
    {
        if (inventoryM == null)
            inventoryM = CreateModel();
        return inventoryM;
    }

    public InventoryController GetController()
    {
        if (inventoryC == null)
            inventoryC = CreateController();
        return inventoryC;
    }

    private InventoryController CreateController() => new InventoryController(GetModel());

    private InventoryModel CreateModel() =>
        new InventoryModel(inventoryV.SlotVs.Select(view => new SlotModel(view.Type)),
                           inventoryV.GridVs.Select(view => new GridModel(view.Size)));
}
