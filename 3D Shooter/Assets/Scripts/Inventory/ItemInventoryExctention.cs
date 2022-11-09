using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventoryExctention : MonoBehaviour
{
    public Vector2Int Size => _size;

    [SerializeField] private Vector2Int _size;
}