using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Cell : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public event Action<Cell> OnClick;

    public bool IsFree = true;

    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _text;

    private InventoryItem item;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_text)
            Debug.Log("Нажали на кнопку: " + _text.text);
        OnClick.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetActiveColor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetPasiveColor();
    }

    public void SetActiveColor()
    {
        _image.color = IsFree ? Color.green : Color.red;
    }

    public void SetPasiveColor()
    {
        _image.color = IsFree ? Color.white : Color.grey;
    }

    public void SetText(string str)
    {
        _text.text = str;
    }

    public void SetItem(InventoryItem newItem)
    {
        IsFree = false;
        if (newItem.IsInstantiate)
            item = newItem;
        else
        {
            item = Instantiate(newItem, transform);
            item.IsInstantiate = true;

        }
    }

    public InventoryItem GetItem() => item;

    public void RemoveItem()
    {
        IsFree = true;
        item = null;
    }
}
