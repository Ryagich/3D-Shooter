using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTargetLooker : MonoBehaviour
{
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
