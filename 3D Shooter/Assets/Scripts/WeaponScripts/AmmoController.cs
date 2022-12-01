using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(AdditionalItemData))]
public class AmmoController : MonoBehaviour
{
    public event Action OnReload;
    public bool HasAmmo => CurrentAmmo > 0;
    public int CurrentAmmo;
    public int Magazine => _magazine;

    [SerializeField] private int _magazine = 30;
    [SerializeField] private ItemData _itemData;

    private InventoryModel inventoryM;
    private AdditionalItemData additionalItemData;

    private void Start()
    {
        additionalItemData = GetComponent<AdditionalItemData>();
    }

    public void Init(InventoryModel model)
    {
        inventoryM = model;
    }

    public int GetAmmo()
    {
        var toReturn = CurrentAmmo;
        SubtractAmmo(CurrentAmmo);
        return toReturn;
    }

    public void SubtractAmmo(int value = 1)
    {
        CurrentAmmo -= value;
    }

    public void Reload()
    {
        var items = inventoryM.items.Where(a => a.ItemData == _itemData).ToList();
        items.Sort((a, b) => b.Stack.CompareTo(a.Stack));
        foreach (var item in items)
        {
            var amount = Mathf.Min(_magazine - CurrentAmmo, item.Stack);
            item.SetStack(item.Stack - amount);
            CurrentAmmo += amount;
            if (CurrentAmmo == _magazine)
                break;
        }
        OnReload?.Invoke();
    }

    public int GetTotalCount() => inventoryM.items.Where(a => a.ItemData == _itemData)
                                                  .Select(a => a.Stack)
                                                  .Sum();
}
