using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AmmoController : MonoBehaviour
{
    private const string CurrentAmmoKey = nameof(CurrentAmmoKey);
    public event Action OnReload;

    public bool IsReload = false;
    public bool HasAmmo => CurrentAmmo > 0;
    public bool HasFullMagazine => CurrentAmmo == _magazine;
    public int Magaine => _magazine;
    public int CurrentAmmo
    {
        get
        {
            var addData = handItem.ItemM.AdditionalData;
            if (addData.ContainsKey(CurrentAmmoKey))
                return int.Parse(addData[CurrentAmmoKey]);
            return 0;
        }
        set => handItem.ItemM.AdditionalData[CurrentAmmoKey] = value.ToString();
    }

    [SerializeField] private int _magazine = 30;
    [SerializeField] private ItemData _itemData;

    private InventoryModel inventoryM;
    private HandItem handItem;

    public void Init(InventoryModel model)
    {
        inventoryM = model;
        handItem = GetComponent<HandItem>();
    }

    public int GetAmmo()
    {
        var toReturn = CurrentAmmo;
        SubtractAmmo(CurrentAmmo);
        return toReturn;
    }

    public void SubtractAmmo(int value = 1) => CurrentAmmo -= value;

    public void Reload()
    {
        var items = inventoryM.GetItems().Where(a => a.ItemData == _itemData).ToList();
        items.Sort((a, b) => b.Amount.CompareTo(a.Amount));
        foreach (var item in items)
        {
            var amount = Mathf.Min(_magazine - CurrentAmmo, item.Amount);
            item.Amount -= amount;
            CurrentAmmo += amount;
            if (CurrentAmmo == _magazine)
                break;
        }
        IsReload = false;
        OnReload?.Invoke();
    }

    public int GetTotalCount() => inventoryM.GetItems()
                                            .Where(a => a.ItemData == _itemData)
                                            .Select(a => a.Amount)
                                            .Sum();
}
