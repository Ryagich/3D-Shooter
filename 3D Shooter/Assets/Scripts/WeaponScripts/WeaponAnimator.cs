using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{
    [SerializeField] private Transform _targetLook;

    private void FixedUpdate() 
    {
        transform.LookAt(_targetLook);
    }

    public void SetTarget(Transform target)
    {
        _targetLook = target;
    }
}
