using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ModelInteract : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    [SerializeField] private InventoryView View;

    private IItemContainerView gridView;
    private RectTransform rectTransform;
    private bool isActive;

    private void Awake()
    {
        gridView = GetComponent<IItemContainerView>();
        rectTransform = GetComponent<RectTransform>();
        isActive = false;
    }

    private void FixedUpdate()
    {
        var pos = Input.mousePosition;
        var hit = RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, pos, null, out Vector2 local);
        if (hit)
        {
            var newIsActive = rectTransform.rect.Contains(local);
            if (newIsActive != isActive)
                View.ViewObj = newIsActive ? gridView : null;
            isActive = newIsActive;
        }
    }
}
