using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridView : MonoBehaviour, IItemContainerView
{
    public static Vector2 TileSize { get; private set; }
    public GridModel GridModel => gridModel;

    [SerializeField] Vector2 _tileSize = new Vector2(32.0f, 32.0f);
    [SerializeField] private GridModel gridModel;

    private RectTransform rectTrans;

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        Init();
        foreach (var item in gridModel.Items)
            item.UpdatePositionOnGrid();
    }

    public void Init()
    {
        TileSize = _tileSize;
        rectTrans = GetComponent<RectTransform>();
        rectTrans.sizeDelta = Vector2.Scale(gridModel.Size, TileSize);
    }

    public Vector2Int GetGridPosition(Vector2 mousePos)
    {
        var localPos = rectTrans.InverseTransformPoint(mousePos);
        var gridPos = new Vector2Int((int)(localPos.x / TileSize.x),
                                    -(int)(localPos.y / TileSize.y));
        return gridPos;
    }

    public Vector2 CalculatePositionOnGrid(InventoryItem item, Vector2Int pos)
    => new Vector2((pos.x + item.Width / 2f) * TileSize.x,
                 -(pos.y + item.Height / 2f) * TileSize.y);

    public IItemContainerModel GetModel() => gridModel;

    public Transform GetTransform()
    {
        return transform;
    }
}
