using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryStateChanger : MonoBehaviour
{
    [SerializeField] private InventoryView _inventoryV;

    private void OnEnable()
    {
        InputHandler.IDowned += _inventoryV.ChangeInventoryState;
    }

    private void OnDisable()
    {
        InputHandler.IDowned -= _inventoryV.ChangeInventoryState;
    }
}
