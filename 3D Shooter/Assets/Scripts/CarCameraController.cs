using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarCameraController : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _speed;
    
    private GameObject _car;
    private Rigidbody _rb;
    private bool _isCarInitialized;

    public void InitCar(GameObject car)
    {
        Debug.Log("here");
        _car = car;
        _rb = _car.GetComponent<Rigidbody>();
        _isCarInitialized = true;
    }

    public void DeinitCar()
    {
        Debug.Log("there");
        _car = null;
        _rb = null;
        _isCarInitialized = false;
    }

    public void ChangeCamera()
    {
        gameObject.GetComponent<Camera>().depth = gameObject.GetComponent<Camera>().depth == -1 ? 1 : -1;
    }

    void FixedUpdate()
    {
        if (!_isCarInitialized) return;
        var carForward = (_rb.velocity + _car.transform.forward).normalized;
        transform.position = Vector3.Lerp(transform.position,
            _car.transform.position + _car.transform.TransformVector(_offset) + carForward,
            _speed * Time.deltaTime);
        transform.LookAt(_car.transform);
    }
}
