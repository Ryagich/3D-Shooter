using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridView : MonoBehaviour, IItemContainerView
{
    public GridModel GridModel => gridModel;
    
    [SerializeField] private GridModel gridModel;

    private RectTransform rectTrans;
    private Vector2 tileSize;

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        foreach (var item in gridModel.Items)
        {
            if (!item.IsDestroyed)
            item.UpdatePositionOnGrid();
        }
    }

    public void Init()
    {
        tileSize = InventoryController.TileSize;
        rectTrans = GetComponent<RectTransform>();
        rectTrans.sizeDelta = Vector2.Scale(gridModel.Size, tileSize);
    }

    public Vector2Int GetGridPosition(Vector2 mousePos)
    {
        var localPos = rectTrans.InverseTransformPoint(mousePos);
        var gridPos = new Vector2Int((int)(localPos.x / tileSize.x),
                                    -(int)(localPos.y / tileSize.y));
        return gridPos;
    }

    public Vector2 CalculatePositionOnGrid(InventoryItem item, Vector2Int pos)
    => new Vector2((pos.x + item.Width / 2f) * tileSize.x,
                 -(pos.y + item.Height / 2f) * tileSize.y);

    public IItemContainerModel GetModel() => gridModel;

    public Transform GetTransform()
    {
        return transform;
    }
}
