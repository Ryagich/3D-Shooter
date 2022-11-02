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
    private float _brakeInput;
    private Vector3 _speedVector;
    private bool _isHandBraking;
    private float _curBrakeForce;
    private float _steerAngle;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = _centerOfMass.localPosition;
    }

    private void FixedUpdate()
    {
        _speedVector = rb.velocity;
        //Debug.Log(_speedVector.magnitude);
        GetInput();
        HandleMotor();
        HandleBraking();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        _verticalInput = Input.GetAxisRaw("Vertical");
        _horizontalInput = Input.GetAxis("Horizontal");
        _isHandBraking = Input.GetKey(KeyCode.Space);
        
        var angle = Vector3.Angle(transform.forward, _speedVector);
       //Debug.Log(angle);
       //if (angle < 90f)
       //{
       //    if (_verticalInput < 0)
       //    {
       //        _brakeInput = Mathf.Abs(_verticalInput);
       //        _verticalInput = 0;
       //    }
       //    else
       //    {
       //        _brakeInput = 0;
       //    }
       //}
       //else
       //{
       //    if (_verticalInput > 0)
       //    {
       //        _brakeInput = Mathf.Abs(_verticalInput);
       //        _verticalInput = 0;
       //    }
       //    else
       //    {
       //        _brakeInput = 0;
       //    }
       //}

        if ((angle - 90) * _verticalInput > 0)
        {
            _brakeInput = Mathf.Abs(_verticalInput);
            _verticalInput = 0;
        }
        else
        {
            _brakeInput = 0;
        }
    }

    private void HandleMotor()
    {
        //Debug.Log(_frontLeftWheelCollider.rpm);
        _frontLeftWheelCollider.motorTorque = _verticalInput * _motorForce;
        _frontRightWheelCollider.motorTorque = _verticalInput * _motorForce;
        _rearLeftWheelCollider.motorTorque = _verticalInput * _motorForce;
        _rearRightWheelCollider.motorTorque = _verticalInput * _motorForce;
        
        _rearLeftWheelCollider.brakeTorque = _isHandBraking ? _brakeForce * 10 : 0;
        _rearRightWheelCollider.brakeTorque = _isHandBraking ? _brakeForce * 10 : 0;
    }

    private void HandleBraking()
    {
        _frontLeftWheelCollider.brakeTorque = 0.7f * _brakeInput * _brakeForce;
        _frontRightWheelCollider.brakeTorque = 0.7f * _brakeInput * _brakeForce;
        _rearLeftWheelCollider.brakeTorque = 0.3f * _brakeInput * _brakeForce;
        _rearRightWheelCollider.brakeTorque = 0.3f * _brakeInput * _brakeForce;
    }

    private void HandleSteering()
    {
        _steerAngle = _horizontalInput * _maxSteerAngle;
        _frontLeftWheelCollider.steerAngle = Mathf.Lerp(_frontLeftWheelCollider.steerAngle, _steerAngle, 0.5f);
        _frontRightWheelCollider.steerAngle = Mathf.Lerp(_frontLeftWheelCollider.steerAngle, _steerAngle, 0.5f);
    }

    private void GetTorque(WheelCollider wheel)
    {
        //if (_verticalInput * wheel.rpm >= 0)
        //{
        //    var moveCoefficient = _verticalInput > 0 ? 1f : 0.1f;
        //    wheel.motorTorque = _verticalInput * moveCoefficient * _motorForce;
        //    wheel.brakeTorque = 0;
        //}

        //if (_verticalInput * wheel.rpm < 0)
        //{
        //    wheel.motorTorque = 0;
        //    wheel.brakeTorque = _brakeForce;
        //}
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
