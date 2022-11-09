using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotView : MonoBehaviour, IItemContainerView
{
    [SerializeField] private SlotModel _model;

    private void OnEnable()
    {
        var item = _model.GetItem(Vector2Int.zero);
        if (item)
            item.UpdatePositionOnGrid();
    }

    public Vector2 CalculatePositionOnGrid(InventoryItem item, Vector2Int pos) => Vector2.zero;

    public Vector2Int GetGridPosition(Vector2 mousePos) => Vector2Int.zero;

    public IItemContainerModel GetModel() => _model;

    public Transform GetTransform() => transform;
}
