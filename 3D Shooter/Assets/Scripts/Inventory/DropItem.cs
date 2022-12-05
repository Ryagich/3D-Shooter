using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] private ItemData _data;

    private ItemModel itemM;

    public void AddInInventory(GameObject hero, GameObject _)
    {
        if (itemM == null)
            itemM = new ItemModel(_data);

        var inventoryCreator = hero.GetComponent<InventoryCreator>();

        if (inventoryCreator.GetController().MovePossible(itemM))
            Destroy(gameObject);
    }

    public DropItem InstantiateDropItem(ItemModel itemM, Transform pos = null)
    {
        if (!pos)
            pos = GameObject.FindGameObjectWithTag("Hero").transform;
        this.itemM = itemM;
        return Instantiate(this, pos.position, pos.rotation);
    }
}
