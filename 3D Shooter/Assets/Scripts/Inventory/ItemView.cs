using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemView : MonoBehaviour
{
    public RectTransform Rect { get; private set; }
    public RectTransform TmpRect { get; private set; }

    public static implicit operator bool(ItemView exists) => (exists as UnityEngine.Object) && exists.Model != null && exists.Model.Amount != 0;

    public bool Exists => this && Model != null && Model.Amount != 0;

    [SerializeField] private TMP_Text _text;

    private InventoryView inventoryV;
    public ItemModel Model { get; private set; }

    private void Awake()
    {
        Rect = GetComponent<RectTransform>();
        TmpRect = _text.GetComponent<RectTransform>();
    }

    private void UpdateText()
    {
        _text.text = Model.Amount.ToString();
        _text.gameObject.SetActive(Model.Amount != 1);
    }

    public void SetView(InventoryView inventoryV)
    {
        this.inventoryV = inventoryV;
    }

    public void SetModel(ItemModel itemM)
    {
        Model = itemM;
        UpdateView();
    }

    public void UpdateSize()
    {
        var ts = inventoryV.TileSize;
        if (Model.IsRotated)
            (ts.x, ts.y) = (ts.y, ts.x);
        Rect.sizeDelta = Vector2.Scale(ts, Model.ItemData.Size);
    }

    public void UpdateRotation()
    {
        Rect.rotation = Quaternion.Euler(0, 0, Model.IsRotated ? 90.0f : 0.0f);
        TmpRect.rotation = Quaternion.identity;
        TmpRect.anchoredPosition = Vector2.zero;
        UpdateSize();
    }

    public void UpdateView()
    {
        if (Model == null || Model.Amount == 0)
        {
            if (this as UnityEngine.Object)
                Destroy(gameObject);
            return;
        }
        GetComponent<Image>().sprite = Model.ItemData.ItemIcon;
        UpdateText();
        UpdateSize();
        UpdateRotation();
    }
}
