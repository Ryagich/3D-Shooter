using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] private UnityEvent<GameObject, GameObject> _objEvent;

    public void Press(GameObject hero)
    {
        _objEvent?.Invoke(hero, gameObject);
    }
}
