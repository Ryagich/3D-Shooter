using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    [SerializeField] private int  _maxAmount = 4, _defaultAmount = 1;
    [SerializeField] private Vector2Int _size = new Vector2Int(1,1);
    [SerializeField] private Sprite _itemIcon;
    [SerializeField] private ItemType _type = ItemType.None;
    [SerializeField] private DropItem _dropItem;
    [SerializeField] private HandItem _handItem;
    [Header("key:value")]
    [SerializeField] private List<string> _additionalData = new();
    public HandItem HandItem => _handItem;
    public DropItem DropItem => _dropItem;
    public int Width => _size.x;
    public int Height => _size.y;
    public int MaxAmount => _maxAmount;
    public int DefaultAmount => _defaultAmount;
    public Sprite ItemIcon => _itemIcon;
    public Vector2Int Size => _size;
    public Vector2Int RotatedSize => new Vector2Int(Height, Width);
    public ItemType Type => _type;

    public IEnumerable<string> AdditionalData => _additionalData;
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
    Ammo,
}

