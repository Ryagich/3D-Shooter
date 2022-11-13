using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum WheelDriveType
{
    ForwardWheelDrive,
    RearWheelDrive,
    FullWheelDrive
}
public class CarControl : MonoBehaviour
{
    [SerializeField] private float _motorForce;
    [SerializeField] private float _brakeForce;
    [SerializeField] private float _maxSteerAngle;
    [SerializeField] private WheelDriveType _wheelDriveType;
    
    [SerializeField] private Transform _centerOfMass;
    [SerializeField] public Transform enterPos;
    [SerializeField] public Transform seatPos;
    
    [SerializeField] private WheelCollider _frontLeftWheelCollider;
    [SerializeField] private WheelCollider _frontRightWheelCollider;
    [SerializeField] private WheelCollider _rearLeftWheelCollider;
    [SerializeField] private WheelCollider _rearRightWheelCollider;
    
    [SerializeField] private Transform _frontLeftWheelTransform;
    [SerializeField] private Transform _frontRightWheelTransform;
    [SerializeField] private Transform _rearLeftWheelTransform;
    [SerializeField] private Transform _rearRightWheelTransform;

    [SerializeField] private Transform _steeringWheel;

    [SerializeField] private UnityEvent<GameObject, GameObject> _objEvent;
    
    private float _verticalInput;
    private float _horizontalInput;
    private float _brakeInput;
    private Vector3 _speedVector;
    private bool _isHandBraking;
    private float _curBrakeForce;
    private float _steerAngle;
    private Rigidbody _rb;
    private GameObject _player;

    public void InitPlayer(GameObject hero, GameObject _)
    {
        _player = hero;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.centerOfMass = _centerOfMass.localPosition;
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        _speedVector = _rb.velocity;
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

        if ((angle - 90) * _verticalInput > 0)
        {
            _brakeInput = Mathf.Abs(_verticalInput);
            _verticalInput = 0;
        }
        else
        {
            _brakeInput = 0;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            _objEvent?.Invoke(_player, gameObject);
            _frontLeftWheelCollider.motorTorque = 0;
            _frontRightWheelCollider.motorTorque = 0;
            _rearLeftWheelCollider.motorTorque = 0;
            _rearRightWheelCollider.motorTorque = 0;
            _frontLeftWheelCollider.brakeTorque = 0.7f * _brakeForce;
            _frontRightWheelCollider.brakeTorque = 0.7f * _brakeForce;
            _rearLeftWheelCollider.brakeTorque = 0.3f * _brakeForce;
            _rearRightWheelCollider.brakeTorque = 0.3f * _brakeForce;
        }
    }

    private void HandleMotor()
    {
        switch (_wheelDriveType)
        {
            case WheelDriveType.ForwardWheelDrive:
                _frontLeftWheelCollider.motorTorque = 2 * _verticalInput * _motorForce;
                _frontRightWheelCollider.motorTorque = 2 * _verticalInput * _motorForce;
                break;
            case WheelDriveType.RearWheelDrive:
                _rearLeftWheelCollider.motorTorque = 2 * _verticalInput * _motorForce;
                _rearRightWheelCollider.motorTorque = 2 * _verticalInput * _motorForce;
                break;
            case WheelDriveType.FullWheelDrive:
                _frontLeftWheelCollider.motorTorque = _verticalInput * _motorForce;
                _frontRightWheelCollider.motorTorque = _verticalInput * _motorForce;
                _rearLeftWheelCollider.motorTorque = _verticalInput * _motorForce;
                _rearRightWheelCollider.motorTorque = _verticalInput * _motorForce;
                break;
        }
    }

    private void HandleBraking()
    {
        _frontLeftWheelCollider.brakeTorque = 0.7f * _brakeInput * _brakeForce;
        _frontRightWheelCollider.brakeTorque = 0.7f * _brakeInput * _brakeForce;
        _rearLeftWheelCollider.brakeTorque = 0.3f * _brakeInput * _brakeForce;
        _rearRightWheelCollider.brakeTorque = 0.3f * _brakeInput * _brakeForce;
        
        _rearLeftWheelCollider.brakeTorque = _isHandBraking ? _brakeForce * 10 : 0;
        _rearRightWheelCollider.brakeTorque = _isHandBraking ? _brakeForce * 10 : 0;
    }

    private void HandleSteering()
    {
        _steerAngle = _horizontalInput * _maxSteerAngle;
        _frontLeftWheelCollider.steerAngle = Mathf.Lerp(_frontLeftWheelCollider.steerAngle, _steerAngle, 0.5f);
        _frontRightWheelCollider.steerAngle = Mathf.Lerp(_frontLeftWheelCollider.steerAngle, _steerAngle, 0.5f);
        
        var curRot = _steeringWheel.localRotation.eulerAngles;
        var rotAngle = curRot.z;
        Debug.Log(rotAngle);
        rotAngle = Mathf.LerpAngle(rotAngle, -10 * _steerAngle, 0.5f);
        _steeringWheel.localRotation = Quaternion.Euler(new Vector3(curRot.x, curRot.y, rotAngle)); 
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
