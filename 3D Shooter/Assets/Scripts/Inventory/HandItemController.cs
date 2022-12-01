using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandItemController : MonoBehaviour
{
    public event Action<HandItem> OnChangeHandItem;

    [SerializeField] private Transform _parentTrans;
    [SerializeField] private InventoryController controller;
    [SerializeField] private List<SlotModel> _slots;
    [SerializeField] private HandItem[] items;
    [SerializeField] private int handIndex;

    private void Awake()
    {
        InputHandler.OnFirstWeapon += () => SetHandIndex(0);
        InputHandler.OnSecondWeapon += () => SetHandIndex(1);
        InputHandler.OnTrirdWeapon += () => SetHandIndex(2);

        items = new HandItem[_slots.Count];
        for (int i = 0; i < _slots.Count; i++)
        {
            var j = i;
            _slots[i].OnPlaced += (item) => CreateHandItem(item, j);
            _slots[i].OnRemoved += () => DeleteHandItem(j);
        }
    }

    private void SetHandIndex(int index)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i])
                items[i].gameObject.SetActive(false);
        }
        if (items[index])
            items[index].gameObject.SetActive(true);
        handIndex = index;
        OnChangeHandItem?.Invoke(items[handIndex]);
    }

    private void CreateHandItem(InventoryItem item, int index)
    {
        if (index == handIndex)
            DeleteHandItem(index);
        var newHand = item.InstantiateHand(_parentTrans);
        items[index] = newHand;
        SetHandIndex(handIndex);
    }

    private void DeleteHandItem(int index)
    {
        if (!items[index])
            return;
        Destroy(items[index].gameObject);
        items[index] = null;
    }
}
