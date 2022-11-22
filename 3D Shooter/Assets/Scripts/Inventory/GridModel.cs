using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GridModel : MonoBehaviour, IItemContainerModel
{
    public Vector2Int Size => _size;
    public GridView GridView => _view;

    [SerializeField] private Vector2Int _size;
    [SerializeField] private GridView _view;

    [SerializeField] private Transform _parent;

    private InventoryItem[,] inventorySlots;
    private List<InventoryItem> items = new List<InventoryItem>();

    private void Awake()
    {
        Init(_size);
    }

    public void Init(Vector2Int size)
    {
        _size = size;
        inventorySlots = new InventoryItem[_size.x, _size.y];
        items = new List<InventoryItem>();
    }

    public IEnumerable<InventoryItem> Items => items;

    public Vector2Int? GetFreePositon(Vector2Int size, ItemData _)
    {
        if (Size == Vector2Int.zero)
            return null;
        var searchSize = _size - size;

        for (int y = 0; y <= searchSize.y; y++)
            for (int x = 0; x <= searchSize.x; x++)
                if (IsFree(new RectInt(new Vector2Int(x, y), size)))
                    return new Vector2Int(x, y);
        return null;
    }

    private bool IsFree(RectInt bounds)
    {
        if (Size == Vector2Int.zero)
            return false;
            for (int x = 0; x < bounds.width; x++)
            for (int y = 0; y < bounds.height; y++)
                if (inventorySlots[bounds.xMin + x, bounds.yMin + y])
                    return false;
        return true;
    }

    private List<InventoryItem> GetItems(RectInt bounds)
    {
        var items = new List<InventoryItem>();
        for (int x = 0; x < bounds.width; x++)
            for (int y = 0; y < bounds.height; y++)
            {
                var item = inventorySlots[bounds.xMin + x, bounds.yMin + y];
                if (item && !items.Contains(item))
                    items.Add(item);
            }
        return items;
    }

    public void PlaceItem(InventoryItem item, Vector2Int pos)
    {
        item.Put(this, _view.transform, pos);
        FillBounds(item, item.GridBounds);        
        item.UpdatePositionOnGrid();
        items.Add(item);
    }

    public void RemoveItem(InventoryItem item)
    {
        ClearItemBounds(item);
        item.Put(null, _parent, Vector2Int.zero);
        items.Remove(item);
    }

    public InventoryItem GetSwapItem(RectInt bounds, InventoryItem _)
    {
        var items = GetItems(bounds);
        if (items.Count != 1)
            return null;
        return items[0];
    }

    public bool CanBePlaced(RectInt bounds, InventoryItem _) => IsFree(bounds);

    private void FillBounds(InventoryItem item, RectInt bounds)
    {
        for (int x = 0; x < bounds.width; x++)
            for (int y = 0; y < bounds.height; y++)
                inventorySlots[bounds.xMin + x, bounds.yMin + y] = item;
    }

    private void ClearItemBounds(InventoryItem item)
    {
        FillBounds(null, item.GridBounds);
    }

    public InventoryItem GetItem(Vector2Int pos) =>
           inventorySlots[Mathf.Clamp(pos.x, 0, inventorySlots.GetLength(0) - 1),
                          Mathf.Clamp(pos.y, 0, inventorySlots.GetLength(1) - 1)];

    public bool IsInBounds(Vector2Int pos) => pos.x >= 0 && pos.x < _size.x
                                            && pos.y >= 0 && pos.y < _size.y;

    public bool IsInBounds(RectInt bounds) =>
                IsInBounds(bounds.min)
             && IsInBounds(bounds.max - Vector2Int.one);

    public IItemContainerView GetView() => _view;
}
