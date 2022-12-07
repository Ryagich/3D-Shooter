using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotExtentionModel : SlotModel
{
    private const string ExctentionXKey = nameof(ExctentionXKey);
    private const string ExctentionYKey = nameof(ExctentionYKey);

    public event Action<GridModel, Vector2Int> OnGridResize;
    private readonly GridModel gridM;

    public SlotExtentionModel(ItemType type, GridModel gridM) : base(type)
    {
        this.gridM = gridM;

        base.OnPlaced += OnPlaced;
        base.OnRemoved += () => OnGridResize?.Invoke(gridM, Vector2Int.zero);
    }

    private void OnPlaced(ItemModel itemM)
    {
        var addData = itemM.AdditionalData;
        if (!addData.ContainsKey(ExctentionXKey)
         || !addData.ContainsKey(ExctentionYKey))
            throw new InvalidOperationException("No extention found");

        var size = new Vector2Int(int.Parse(addData[ExctentionXKey]),
                                  int.Parse(addData[ExctentionYKey]));
        OnGridResize?.Invoke(gridM, size);
    }
}
