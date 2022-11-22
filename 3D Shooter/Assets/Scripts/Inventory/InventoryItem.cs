using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour
{
    public event Action OnPutInGrid;
    public event Action OnPutInSlot;
    public event Action OnPickUp;
    public event Action<InventoryItem> OnDelete;

    public int MaxStack;
    public int Stack;
    public Vector2Int GridPos;
    public Vector2Int LastGridPos;

    private IItemContainerModel model;
    private IItemContainerModel lastModel;

    [SerializeField] private TMP_Text _text;

    public bool IsRotated { get; private set; }
    public ItemData ItemData { get; private set; }
    public int Height => IsRotated ? ItemData.Width : ItemData.Height;
    public int Width => IsRotated ? ItemData.Height : ItemData.Width;
    public bool IsSingle => Stack == 1;
    public bool IsMax => MaxStack == Stack;
    public int HowMuchCanAdd => MaxStack - Stack;
    public Vector2Int Size => new Vector2Int(Width, Height);
    public RectInt GridBounds => new RectInt(GridPos, Size);
    public bool IsPlaced => model != null;
    public IItemContainerModel GetLastModel => lastModel;

    public bool CanBeAddedOnStack(InventoryItem item) => item.ItemData.Type == ItemData.Type && !IsMax;
    public bool CanBeAddedOnStack(ItemData data) => data.Type == ItemData.Type && !IsMax;

    public int FillStack(int stack)
    {
        if (stack <= HowMuchCanAdd)
        {
            AddStack(stack);
            return 0;
        }
        stack -= HowMuchCanAdd;
        SetStack(MaxStack);
        return stack;
    }

    public void SetStack(int newstack)
    {
        Stack = newstack;
        UpdateText();
        if (Stack == 0)
            Delete();
    }

    public void AddStack(int value)
    {
        Stack += value;
        UpdateText();
    }

    public void SubtractStack(int value)
    {
        Stack -= value;
        UpdateText();
        if (Stack < 1)
        {
            model?.RemoveItem(this);
            Delete();
        }
    }

    private void UpdateText()
    {
        _text.text = Stack.ToString();
        _text.gameObject.SetActive(Stack != 1);
    }

    public InventoryItem GetHalf()
    {
        var toReturn = Instantiate(ItemData.ItemPref, transform.parent);
        toReturn.SetData(ItemData);
        var half = Stack / 2;
        toReturn.Stack = half;
        Stack -= half;
        UpdateText();
        return toReturn;
    }

    public void UpdatePositionOnGrid()
    {
        if (IsPlaced)
        {
            var slot = model as SlotModel;
            transform.localPosition = model.GetView().CalculatePositionOnGrid(this, GridPos);
            if (slot != null)
            {
                var v = slot.GetView().GetTransform().GetComponent<RectTransform>().sizeDelta;
                if ((ItemData.Size.x > ItemData.Size.y) ^ IsRotated ^ (v.x > v.y))
                    Rotate();
            }
        }
        UpdateSize();
    }

    public void Put(IItemContainerModel newModel, Transform parent, Vector2Int pos)
    {
        lastModel = model;
        LastGridPos = GridPos;

        model = newModel;
        transform.SetParent(parent);
        GridPos = pos;

        if (model == null)
            OnPickUp?.Invoke();
        else if (model as SlotModel != null)
            OnPutInSlot?.Invoke();
        else
            OnPutInGrid?.Invoke();
    }

    public void Delete()
    {
        Destroy(gameObject);
        OnDelete?.Invoke(this);
    }

    public void UpdateSize()
    {
        var ts = GridView.TileSize;
        if (IsRotated)
            (ts.x, ts.y) = (ts.y, ts.x);
        GetComponent<RectTransform>().sizeDelta = Vector2.Scale(ts, ItemData.Size);
    }

    public void SetData(ItemData itemData)
    {
        ItemData = itemData;
        MaxStack = itemData.MaxStack;
        Stack = itemData.Stack;
        GetComponent<Image>().sprite = itemData.ItemIcon;
        GetComponent<RectTransform>().sizeDelta = Vector2.Scale(GridView.TileSize, Size);
        UpdateText();
    }

    public void Rotate()
    {
        if (Height == Width)
            return;

        IsRotated = !IsRotated;
        var rectTransform = GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(0, 0, IsRotated ? 90.0f : 0.0f);
        var rect = _text.GetComponent<RectTransform>();
        _text.transform.rotation = Quaternion.identity;
        rect.anchoredPosition = Vector2.zero;

        UpdateSize();
    }
}
