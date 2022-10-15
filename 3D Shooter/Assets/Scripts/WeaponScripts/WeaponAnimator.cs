using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{
    [SerializeField] private Transform _targetLook, _shootPoint, _cameraTransform;
    private void FixedUpdate() 
    {
        Debug.DrawLine(_cameraTransform.position, _targetLook.position, Color.green);   
        Debug.DrawLine(_shootPoint.position, _targetLook.position, Color.red);   

        transform.LookAt(_targetLook);
    }
}
