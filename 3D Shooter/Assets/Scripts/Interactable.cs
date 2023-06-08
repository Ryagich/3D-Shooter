using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] private UnityEvent<GameObject, GameObject> _objEvent;

    private bool isPressed = false;
    public bool multipleUses;

    public void Press(GameObject hero)
    {
        if (!isPressed || multipleUses)
        {
            isPressed = true;
            _objEvent?.Invoke(hero, gameObject);
        }
    }
}
