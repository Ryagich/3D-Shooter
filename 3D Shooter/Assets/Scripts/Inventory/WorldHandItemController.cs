using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class WorldHandItemController : MonoBehaviour
{
    public event Action<HandItem> OnChangeHandItem;

    [SerializeField] private Transform _parentTrans;
    [SerializeField] private InventoryController controller;
    [SerializeField] private HandItem[] items;
    [SerializeField] private int handIndex;
    [SerializeField] private InventoryCreator inventoryMCreator;

    private List<SlotModel> slots = new List<SlotModel>();

    public HandItem Hand => items[handIndex];

    private void Awake()
    {
        slots = inventoryMCreator.GetModel().SlotMs
                                 .Where(x => x.Type == ItemType.SecondWeapon
                                          || x.Type == ItemType.MainWeapon).ToList();

        InputHandler.FirstWeaponChoosed += () => SetHandIndex(0);
        InputHandler.SecondWeaponChoosed += () => SetHandIndex(1);
        InputHandler.TrirdWeaponChoosed += () => SetHandIndex(2);

        items = new HandItem[slots.Count];
        for (int i = 0; i < slots.Count; i++)
        {
            var j = i;
            slots[i].OnPlaced += (item) => CreateHandItem(item, j);
            slots[i].OnRemoved += () => DeleteHandItem(j);
        }
    }

    private void SetHandIndex(int index)
    {
        for (int i = 0; i < items.Length; i++)
            if (items[i])
                items[i].gameObject.SetActive(false);
        if (items[index])
            items[index].gameObject.SetActive(true);
        handIndex = index;
        OnChangeHandItem?.Invoke(items[handIndex]);
    }

    private void CreateHandItem(ItemModel item, int index)
    {
        if (items[index] && index == handIndex)
            return;
        if (!items[index])
        {
            var newHand = InstantiateHand(item);
            items[index] = newHand;
        }
        SetHandIndex(handIndex);
    }

    private HandItem InstantiateHand(ItemModel itemM)
    {
        var itemHand = Instantiate(itemM.ItemData.HandItem, _parentTrans);
        var modelHolder = itemHand.GetComponent<WorldHandItemModelHolder>();
        if (modelHolder)
            modelHolder.ItemM = itemM;
        return itemHand;
    }

    private void DeleteHandItem(int index)
    {
        if (!items[index])
            return;
        Destroy(items[index].gameObject);
        items[index] = null;
    }
}
