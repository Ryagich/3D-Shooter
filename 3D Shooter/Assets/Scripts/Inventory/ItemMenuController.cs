using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemMenuController : MonoBehaviour
{
    [SerializeField] private Image _back;
    [SerializeField] private Button _dropPref;
    [SerializeField] private Camera _camera;

    [SerializeField] private GameObject menu = null;

    private void OnEnable()
    {
        InputHandler.LeftMouseDowned += OnMouse;
        InputHandler.RightMouseDowned += OnMouse;
    }

    private void OnDisable()
    {
        InputHandler.LeftMouseDowned -= OnMouse;
        InputHandler.RightMouseDowned -= OnMouse;
    }

    private void OnMouse()
    {
        var ray = _camera.ScreenPointToRay(InputHandler.MousePos);
        if (Physics.Raycast(ray, out var hit))
        {
            var objectHit = hit.transform;
            Debug.Log(objectHit.transform.name);
            Debug.Log(objectHit.gameObject + " | " + menu);
            if (menu)
            Debug.Log(objectHit.gameObject.name +" | " + menu.name);

            if (objectHit.gameObject != menu)
            {
                Debug.Log("нажал на " + objectHit.name + " меню закрылось");
                Close();
            }
        }
    }

    public void Open(ItemModel itemM, Vector2 pos)
    {
        menu = Instantiate(_back.gameObject,transform);
        menu.transform.position = pos;
        var dropB = Instantiate(_dropPref, menu.transform);

        dropB.onClick.AddListener(itemM.Remove);
        dropB.onClick.AddListener(Close);
    }

    private void Close()
    {
        Destroy(menu);
        menu = null;
    }
}
