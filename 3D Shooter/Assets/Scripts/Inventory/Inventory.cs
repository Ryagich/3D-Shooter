using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField, Min(0.0f)] private float _maxWeight = 20.0f, _weight = 0.0f;
    [SerializeField] private Transform _HandItemTrans, _targetLook;

    private List<InventoryItem> items = new List<InventoryItem>();
    private HandItem currHandItem;

    public bool CanAdded(float weight) => _maxWeight > weight + _weight;

    public void SetNewHandItem(HandItem item)
    {
        DestroyCurrHandItem();
        currHandItem = Instantiate(item);
        currHandItem.transform.SetParent(_HandItemTrans);
        currHandItem.transform.localPosition = _HandItemTrans.localPosition;
        currHandItem.gameObject?.GetComponent<WeaponAnimator>().SetTarget(_targetLook);

    }

    public void DestroyCurrHandItem()
    {
        if (currHandItem != null)
        {
            if (currHandItem.gameObject != null)
                Destroy(currHandItem.gameObject);
            currHandItem = null;
        }
    }

    public void AddItem(InventoryItem item)
    {
        _weight += item._weight;
        items.Add(item);

        if (item.ItemInfo.HandItem != null && currHandItem == null)
            SetNewHandItem(item.ItemInfo.HandItem);
    }

    public void DeleteItem(InventoryItem item)
    {
        _maxWeight -= item._weight;
        items.Remove(item);
    }
}
