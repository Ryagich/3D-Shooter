using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControl : MonoBehaviour
{
    [SerializeField] private float _motorForce;
    [SerializeField] private float _brakeForce;
    [SerializeField] private float _maxSteerAngle;

    [SerializeField] private Transform _centerOfMass;
    
    [SerializeField] private WheelCollider _frontLeftWheelCollider;
    [SerializeField] private WheelCollider _frontRightWheelCollider;
    [SerializeField] private WheelCollider _rearLeftWheelCollider;
    [SerializeField] private WheelCollider _rearRightWheelCollider;
    
    [SerializeField] private Transform _frontLeftWheelTransform;
    [SerializeField] private Transform _frontRightWheelTransform;
    [SerializeField] private Transform _rearLeftWheelTransform;
    [SerializeField] private Transform _rearRightWheelTransform;
    
    private float _verticalInput;
    private float _horizontalInput;
    private bool _isBraking;
    private float _curBrakeForce;
    private float _steerAngle;

    private void Awake()
    {
        var rb = GetComponent<Rigidbody>();
        rb.centerOfMass = _centerOfMass.localPosition;
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        _verticalInput = Input.GetAxisRaw("Vertical");
        _horizontalInput = Input.GetAxis("Horizontal");
        //_isBraking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        Debug.Log(_frontLeftWheelCollider.rpm);
        GetTorque(_frontLeftWheelCollider);
        GetTorque(_frontRightWheelCollider);
    }

    private void HandleSteering()
    {
        _steerAngle = _maxSteerAngle * _horizontalInput;
        _frontLeftWheelCollider.steerAngle = _steerAngle;
        _frontRightWheelCollider.steerAngle = _steerAngle;
    }

    private void GetTorque(WheelCollider wheel)
    {
        if (_verticalInput * wheel.rpm >= 0)
        {
            wheel.motorTorque = _verticalInput * _motorForce;
            wheel.brakeTorque = 0;
        }

        if (_verticalInput * wheel.rpm < 0)
        {
            wheel.motorTorque = 0;
            wheel.brakeTorque = _brakeForce;
        }
    }

    private void UpdateWheels()
    {
        UpdateWheel(_frontLeftWheelCollider, _frontLeftWheelTransform);
        UpdateWheel(_frontRightWheelCollider, _frontRightWheelTransform);
        UpdateWheel(_rearLeftWheelCollider, _rearLeftWheelTransform);
        UpdateWheel(_rearRightWheelCollider, _rearRightWheelTransform);
    }

    private static void UpdateWheel(WheelCollider col, Transform trans)
    {
        Vector3 pos;
        Quaternion rot;
        col.GetWorldPose(out pos, out rot);
        trans.position = pos;
        trans.rotation = rot;
    }
}
