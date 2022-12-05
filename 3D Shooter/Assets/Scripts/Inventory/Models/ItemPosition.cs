using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPosition
{
    public readonly IItemContainerModel Model;
    public readonly Vector2Int Position;
    public readonly bool IsRotated;

    public ItemPosition(IItemContainerModel model, Vector2Int position, bool isRotated)
    {
        Model = model;
        Position = position;
        IsRotated = isRotated;
    }
}