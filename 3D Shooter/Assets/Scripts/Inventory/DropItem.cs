using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AdditionalItemData))]
public class DropItem : MonoBehaviour
{
    [SerializeField] private ItemData _data;

    public int Stack;

    public void AddInInventory(GameObject hero, GameObject _)
    {
        var model = hero.GetComponent<InventoryController>().Model;
        var item = model.InstantiateAddItem(_data, Stack);       
        Destroy(gameObject);
    }

    public DropItem InstantiateDropItem(int stack)
    {
        Stack = stack;
        var hero = GameObject.FindGameObjectWithTag("Hero").transform;
        return Instantiate(this, hero.position, hero.rotation);
    }

    public DropItem InstantiateDropItem(Transform transform, int stack)
    {
        if (!transform)
            return InstantiateDropItem(stack);
        Stack = stack;
        return Instantiate(this, transform.position, transform.rotation);
    }
}
