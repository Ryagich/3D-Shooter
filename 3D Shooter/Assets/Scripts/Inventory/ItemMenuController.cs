using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.Progress;
using System.Linq;
using TMPro;

public class ItemMenuController : MonoBehaviour
{
    public bool IsOpen => isOpen;

    [SerializeField] private Image _back;
    [SerializeField] private Camera _camera;
    [SerializeField] private InventoryCreator _creator;
    [SerializeField] private WorldHandItemController _worldHandC;
    [Header("Buttons")]
    [SerializeField] private Vector2Int _size = new Vector2Int(100, 30);
    [SerializeField] private TMP_Text _textPref;

    private GameObject menu = null;
    private Vector2 pos = Vector2.zero;
    private Vector2 size = Vector2.zero;
    private List<ItemMenuButton> buttons = new();
    private bool isOpen = false;

    private void OnEnable() => InputHandler.LeftMouseDowned += OnLeftMouse;

    private void OnDisable() => InputHandler.LeftMouseDowned -= OnLeftMouse;

    private void OnLeftMouse()
    {
        if (menu == null)
            return;
        if (!IsInBounds())
        {
            Close();
            return;
        }
        foreach (var button in buttons)
            if (IsInBounds(button.Pos, button.Size))
            {
                button.Click();
                return;
            }
    }

    public bool IsInBounds(Vector2 pos)
    {
        var mp = InputHandler.MousePos; // Left Top Point
        return mp.x >= pos.x && mp.x <= pos.x + size.x &&
               mp.y <= pos.y && mp.y >= pos.y - size.y;
    }

    private bool IsInBounds()
    {
        var mp = InputHandler.MousePos; // Left Top Point
        return mp.x >= pos.x && mp.x <= pos.x + size.x &&
               mp.y <= pos.y && mp.y >= pos.y - size.y;
    }

    private bool IsInBounds(Vector2 pos, Vector2 size)
    {
        var mp = InputHandler.MousePos; // Left Top Point
        return mp.x >= pos.x && mp.x <= pos.x + size.x &&
               mp.y <= pos.y && mp.y >= pos.y - size.y;
    }

    public void Open(ItemModel itemM, Vector2 pos)
    {
        Close();
        isOpen = true;
        menu = Instantiate(_back.gameObject, transform);
        menu.transform.position = pos;
        this.pos = pos;

        CreateButtons(itemM);

        var rect = menu.GetComponent<RectTransform>();
        size = new Vector2(rect.sizeDelta.x, _size.y * buttons.Count);
        rect.sizeDelta = size;
    }

    private void CreateButtons(ItemModel itemM)
    {
        var useB = CreateUseButton(itemM);
        if (useB != null)
            buttons.Add(useB);
        buttons.Add(CreateDropButton(itemM));

        for (int i = 0; i < buttons.Count; i++)
        {
            var b = buttons[i];
            b.ChangePosition(new Vector2(b.Pos.x, b.Pos.y - i * _size.y));
            var text = Instantiate(_textPref, menu.transform);
            text.transform.localPosition = new Vector2(0, -i * _size.y);
            text.text = b.Name;
        }
    }

    private ItemMenuButton CreateDropButton(ItemModel itemM)
    {
        var dropB = new ItemMenuButton(pos, _size, "Drop");
        dropB.Clicked += () => _creator.GetController().Drop(itemM);
        dropB.Clicked += Close;
        return dropB;
    }

    private ItemMenuButton CreateUseButton(ItemModel itemM)
    {
        if (itemM.ItemData.HandItem == null || itemM.ContainerM is SlotModel
         || _worldHandC.Hand && itemM == _worldHandC.Hand.ItemM)
            return null;
        var useB = new ItemMenuButton(pos, _size, "Use");
        useB.Clicked += () => SetHand(itemM);
        useB.Clicked += Close;
        return useB;
    }

    private void SetHand(ItemModel itemM)
    {
        _worldHandC.DeleteAddHand();
        _worldHandC.CreateAddHand(itemM);
        _worldHandC.SetAddHandIndex();
    }


    private void Close()
    {
        isOpen = false;
        Destroy(menu);
        menu = null;
        pos = Vector2.zero;
        size = Vector2.zero;
        buttons = new List<ItemMenuButton>();
    }
}

public class ItemMenuButton
{
    public event Action Clicked;
    public Vector2 Pos => pos;
    public Vector2 Size => size;

    public string Name => name;

    private string name = "no name";
    private Vector2 pos;
    private Vector2Int size;

    public ItemMenuButton(Vector2 pos, Vector2Int size, string name)
    {
        this.pos = pos;
        this.size = size;
        this.name = name;
    }

    public void ChangePosition(Vector2 pos) => this.pos = pos;

    public void Click() => Clicked?.Invoke();
}
