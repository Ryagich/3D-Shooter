using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public Item ItemInfo;
    [Min(1)] public int width, height;
    [Min(0.0f)] public float _weight = 1.0f;
    public Image Icon;
    public InventoryGrid Grid;
    public bool IsInstantiate = false;

}

public enum SlotType
{
    Main = 0,
    Second = 1,
    None,
}

