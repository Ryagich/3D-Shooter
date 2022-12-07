using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemModel
{
    public event Action OnAmountChanged;
    public event Action OnPositionChanged;

    private int amount;

    public readonly ItemData ItemData;
    public bool IsRotated { get; private set; }
    public Vector2Int Position { get; private set; }
    public Vector2Int Size => new Vector2Int(Width, Height);
    public RectInt GridBounds => new RectInt(Position, Size);
    public int Height => IsRotated ? ItemData.Width : ItemData.Height;
    public int Width => IsRotated ? ItemData.Height : ItemData.Width;
    public bool IsPlaced => ContainerM != null;

    public int Amount { get => amount; set => SetAmount(value); }
    public IItemContainerModel ContainerM { get; private set; }
    public int MaxAmount => ItemData.MaxAmount;
    public int FreeAmount => MaxAmount - Amount;
    public bool IsMaxAmount => Amount == MaxAmount;
    public readonly Dictionary<string, string> AdditionalData;

    public ItemModel(ItemData data)
    {
        ItemData = data;
        Amount = ItemData.DefaultAmount;
        IsRotated = false;
        Position = Vector2Int.zero;

        AdditionalData = new Dictionary<string, string>();
        foreach (var field in data.AdditionalData)
        {
            var splited = field.Split(':');
            var key = splited[0];
            var value = splited[1];
            AdditionalData.Add(key, value);
        }
    }

    public static int MoveMaxPossibleAmount(ItemModel from, ItemModel to)
    {
        var moveAmount = Mathf.Min(to.FreeAmount, from.Amount);
        MoveAmount(from, to, moveAmount);
        return moveAmount;
    }

    public static void MoveAmount(ItemModel from, ItemModel to, int amount)
    {
        if (from.ItemData != to.ItemData)
            throw new ArgumentException();
        to.Amount += amount;
        from.Amount -= amount;
    }

    private void SetAmount(int amount)
    {
        if (amount < 0 && amount > MaxAmount)
            throw new ArgumentException();
        this.amount = amount;
        if (amount == 0 && IsPlaced)
            Remove();

        OnAmountChanged?.Invoke();
    }

    public void Remove()
    {
        if (IsPlaced)
            ContainerM.RemoveItem(this);
        else
            throw new InvalidOperationException("Remove from null");
    }

    public void Rotate()
    {
        if (Height == Width)
            return;
        IsRotated = !IsRotated;

        OnPositionChanged?.Invoke();
    }

    public void Put(IItemContainerModel model, Vector2Int pos)
    {
        ContainerM = model;
        Position = pos;

        OnPositionChanged?.Invoke();
    }
}
