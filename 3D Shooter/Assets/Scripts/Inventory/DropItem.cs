using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] private ItemData _data;

    public void AddInInventory(GameObject hero, GameObject _)
    {
        var model = hero.GetComponent<InventoryController>().Model;

        if (model.CanBeAdd(_data))
        {
            model.AddItem(_data);
            Destroy(gameObject);
        }
    }
}
