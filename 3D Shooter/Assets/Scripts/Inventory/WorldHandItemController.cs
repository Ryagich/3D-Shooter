using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using System.Reflection;

public class WorldHandItemController : MonoBehaviour
{
    public event Action<HandItem> OnChangeHandItem;
    public HandItem Hand => items[handIndex];

    [SerializeField] private Transform _parentTrans;
    [SerializeField] private HandItem[] items;
    [SerializeField] private int handIndex = 0;
    [SerializeField] private InventoryCreator inventoryCreator;

    private List<SlotModel> slots = new();

    private void Awake()
    {
        slots = inventoryCreator.GetModel().SlotMs
                                 .Where(x => x.Type == ItemType.SecondWeapon
                                          || x.Type == ItemType.MainWeapon).ToList();

        InputHandler.FirstWeaponChoosed += () => SetHandIndex(0);
        InputHandler.SecondWeaponChoosed += () => SetHandIndex(1);
        InputHandler.TrirdWeaponChoosed += () => SetHandIndex(2);

        items = new HandItem[slots.Count + 1];
        for (int i = 0; i < slots.Count; i++)
        {
            var j = i;
            slots[i].OnPlaced += (item) => CreateHandItem(item, j);
            slots[i].OnRemoved += () => DeleteHandItem(j);
        }
    }

    public void CreateAddHand(ItemModel itemM)
    {
        CreateHandItem(itemM, items.Length - 1);
        itemM.OnPositionChanged += DeleteAddHand;
    }

    public void DeleteAddHand()
    {
        if (items[items.Length - 1])
          items[items.Length - 1].ItemM.OnPositionChanged -= DeleteAddHand;
        DeleteHandItem(items.Length - 1);
    }

    public void SetAddHandIndex() => SetHandIndex(items.Length - 1);

    private void SetHandIndex(int index)
    {
        if (index != items.Length - 1)
            DeleteAddHand();

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
        itemHand.ItemM = itemM;
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
