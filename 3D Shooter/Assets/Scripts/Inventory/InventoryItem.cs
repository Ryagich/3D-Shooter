using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public Item ItemInfo;
    [Min(0.0f)] public float _weight = 1.0f;
}
