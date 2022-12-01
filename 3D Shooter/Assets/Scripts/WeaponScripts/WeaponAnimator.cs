using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{
    public bool IsReloading { get; private set; }

    private Transform _targetLook;

    private void FixedUpdate()
    {
        transform.LookAt(_targetLook);
    }

    public void SetTargetLook(Transform targetLook)
    {
        _targetLook = targetLook;
    }
}
