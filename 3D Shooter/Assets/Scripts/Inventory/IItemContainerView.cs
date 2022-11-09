using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemContainerView 
{
    public Vector2 CalculatePositionOnGrid(InventoryItem item, Vector2Int pos);
    public Vector2Int GetGridPosition(Vector2 mousePos);
    public IItemContainerModel GetModel();
    public Transform GetTransform();
}
