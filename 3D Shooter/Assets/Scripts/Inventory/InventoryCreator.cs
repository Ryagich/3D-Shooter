using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCreator : MonoBehaviour
{
    [SerializeField] private InventoryView inventoryV;

    private InventoryModel inventoryM;
    private InventoryController inventoryC;

    private void Start()
    {
        inventoryV.UpdateModel();
    }

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

    private InventoryModel CreateModel()
    {
        var extGridMs = new List<GridModel>();
        var extGridVs = new List<GridView>();
        var slotMs = new List<SlotModel>();

        foreach (var slotV in inventoryV.SlotVs)
        {
            if (slotV.TryGetComponent<SlotInventoryExtention>(out var extention))
            {
                var gridV = extention.GridV;
                var extGridM = new GridModel(Vector2Int.zero);
                extGridVs.Add(gridV);
                extGridMs.Add(extGridM);
                slotMs.Add(new SlotExtentionModel(slotV.Type, extGridM));
            }
            else
            {
                slotMs.Add(new SlotModel(slotV.Type));
            }
        }

        var gridMs = inventoryV.GridVs
            .Except(extGridVs)
            .Select(view => new GridModel(view.Size))
            .Union(extGridMs)
            .ToList();

        return new InventoryModel(slotMs, gridMs);
    }
}
