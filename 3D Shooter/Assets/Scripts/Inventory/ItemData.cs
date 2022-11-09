using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    [SerializeField] private int _width = 1, _height = 1, _maxStack = 4, _stack = 1;
    [SerializeField] private Sprite _itemIcon;
    [SerializeField] private ItemType _type = ItemType.None;
    [SerializeField] private InventoryItem _itemPref;
    [SerializeField] private DropItem _dropItem;

    public DropItem DropItem => _dropItem;
    public int Width => _width;
    public int Height => _height;
    public int MaxStack => _maxStack;
    public int Stack => _stack;
    public Sprite ItemIcon => _itemIcon;
    public Vector2Int Size => new Vector2Int(Width, Height);
    public Vector2Int RotatedSize => new Vector2Int(Height, Width);
    public InventoryItem ItemPref => _itemPref;
    public ItemType Type => _type;
}

public enum ItemType
{
    None = 0,
    MainWeapon = 1,
    SecondWeapon,
    Food,
    Medical,
    Head,
    Body,
    Legs,
    Feets,
}

