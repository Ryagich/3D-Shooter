using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCameraController : MonoBehaviour
{
    [SerializeField] private Transform _car;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _speed;
    
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = _car.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var carForward = (rb.velocity + _car.transform.forward).normalized;
        transform.position = Vector3.Lerp(transform.position,
            _car.position + _car.transform.TransformVector(_offset) + carForward, _speed * Time.deltaTime);
        transform.LookAt(_car);
    }
}
