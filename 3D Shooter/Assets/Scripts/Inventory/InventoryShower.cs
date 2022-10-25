using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryShower : MonoBehaviour
{
    [SerializeField] private TMP_Text _weightText;
    [SerializeField] private Cell _cellPref;
    [SerializeField] private Cell _mainSlotF, _mainSlotS, _secondSlot;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private float _offset = 1.0f;
    [SerializeField] private List<Cell> cells = new List<Cell>();
    [SerializeField] private InventoryItem hand;

    private void Awake()
    {
        _inventory.OnAdded += AddItem;
        _inventory.OnChangeWeight += UpdateText;

        InstantiateCells();

        foreach (var item in _inventory.Items)
            AddItem(item);
    }

    private void UpdateText(float value)
    {
        _weightText.text = value.ToString();
    }

    private void AddItem(InventoryItem item)
    {
        if (true)
        {

        }

        foreach (var cell in cells)
            if (cell.IsFree)
            {
                cell.SetItem(item);
                break;
            }
    }

    private void OnEnable()
    {
        if (cells.Count == 0)
            return;
        foreach (var cell in cells)
            cell.SetPasiveColor();
    }

    private void Update()
    {
        if (hand)
            hand.transform.position = Input.mousePosition;
    }

    private void CheckCell(Cell cell)
    {
        if (hand)
            if (cell.IsFree)
            {
                cell.SetItem(hand);
                hand = null;
            }
            else
            {
                var newHand = cell.GetItem();
                cell.RemoveItem();
                cell.SetItem(hand);
                hand = newHand;
            }
        else if (!cell.IsFree)
        {
            hand = cell.GetItem();
            cell.RemoveItem();
        }
    }

    public void InstantiateCells()
    {
        var startP = new Vector2(0, 0);
        var grid = _inventory.InventoryGrid.grid;
        for (var i = 0; i < grid.GetLength(0); i++)
            for (var y = 0; y < grid.GetLength(1); y++)
            {
                var cell = Instantiate(_cellPref);
                cells.Add(cell);
                cell.transform.SetParent(transform);
                startP = new Vector2(i * _offset, y * -_offset);
                cell.gameObject.transform.localPosition = startP;
                cell.SetText(i + ";" + y);
                cell.OnClick += CheckCell;
            }
    }
}
