using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] private UnityEvent<GameObject, GameObject> _objEvent;

    private bool isPressed = false;

    public void Press(GameObject hero)
    {
        if (!isPressed)
        {
            isPressed = true;
            _objEvent?.Invoke(hero, gameObject);
        }
    }
}
