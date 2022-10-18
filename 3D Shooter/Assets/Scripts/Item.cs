using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item")]
public class Item : ScriptableObject
{
    public HandItem HandItem;
    public DropItem DropItem;
    public InventoryItem InventoryItem;
}
