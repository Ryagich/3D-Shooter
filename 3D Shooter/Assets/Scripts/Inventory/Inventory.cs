using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event Action<InventoryItem> OnAdded;
    public event Action<float> OnChangeWeight;

    public InventoryGrid InventoryGrid;
    public List<InventoryItem> Items = new List<InventoryItem>();
    public InventoryItem MainSlotF;
    public InventoryItem MainSlotS;
    public InventoryItem SecondSlot;

    [SerializeField, Min(0.0f)] private float _maxWeight = 20.0f, _weight = 0.0f;
    [SerializeField] private Transform _HandItemTrans, _targetLook;
    [SerializeField] private GameObject _inventory;

    private HandItem currHandItem;

    private void Awake()
    {
        InventoryGrid = new InventoryGrid(10, 20);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            _inventory.SetActive(!_inventory.activeSelf);
            Cursor.lockState = _inventory.activeSelf
                             ? CursorLockMode.Confined
                             : CursorLockMode.Locked;
        }
    }

    public bool CanAdded(float weight) => _maxWeight > weight + _weight;

    public void SetNewHandItem(HandItem item)
    {
        DestroyCurrHandItem();
        currHandItem = Instantiate(item, _HandItemTrans);
       // currHandItem.transform.SetParent(_HandItemTrans);
        currHandItem.transform.localPosition = Vector3.zero;
        currHandItem.gameObject?.GetComponent<WeaponAnimator>().SetTarget(_targetLook);
    }

    public void DestroyCurrHandItem()
    {
        if (currHandItem != null)
        {
            Destroy(currHandItem?.gameObject);
            currHandItem = null;
        }
    }

    public void AddItem(InventoryItem item)
    {
        _weight += item._weight;
        Items.Add(item);

        if (item.ItemInfo.HandItem != null && currHandItem == null)
            SetNewHandItem(item.ItemInfo.HandItem);

        OnAdded?.Invoke(item);
        OnChangeWeight?.Invoke(_weight);
    }

    public void DeleteItem(InventoryItem item)
    {
        _maxWeight -= item._weight;
        Items.Remove(item);
        OnChangeWeight?.Invoke(_weight);
    }
}
