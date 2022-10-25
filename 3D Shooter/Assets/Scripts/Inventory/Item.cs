using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item")]
public class Item : ScriptableObject
{
    public string Name;
    public HandItem HandItem;
    public DropItem DropItem;
    public InventoryItem InventoryItem;
    public ItemType Type;
}

public enum ItemType
{
    MainWeapon = 0,
    SecondaryWeapon = 1,
    ColdWeapon,
    Cloth,
    Food,
    Medical,
    Component,
    Armor,
}
