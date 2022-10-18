using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public Item ItemInfo;

    public void OnPickedUp(GameObject hero, GameObject _)
    {
        var inventory = hero.GetComponent<Inventory>();

        if (!inventory.CanAdded(ItemInfo.InventoryItem._weight))
            return;

        inventory.AddItem(ItemInfo.InventoryItem);
        Destroy(gameObject);
    }
}
