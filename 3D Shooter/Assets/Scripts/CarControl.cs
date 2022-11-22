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
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _fuelUsageMultiplier;
    [SerializeField] private WheelDriveType _wheelDriveType;
    
    [SerializeField] private Transform _centerOfMass;
    [SerializeField] public Transform enterPos;
    [SerializeField] public Transform seatPos;
    [SerializeField] public GameObject _ui;
    
    [SerializeField] private WheelCollider _frontLeftWheelCollider;
    [SerializeField] private WheelCollider _frontRightWheelCollider;
    [SerializeField] private WheelCollider _rearLeftWheelCollider;
    [SerializeField] private WheelCollider _rearRightWheelCollider;
    
    [SerializeField] private Transform _frontLeftWheelTransform;
    [SerializeField] private Transform _frontRightWheelTransform;
    [SerializeField] private Transform _rearLeftWheelTransform;
    [SerializeField] private Transform _rearRightWheelTransform;

    [SerializeField] private Transform _steeringWheel;
    [SerializeField] private Transform _fuelScale;
    [SerializeField] private Transform _speedometerArrow;
    
    [SerializeField] private UnityEvent _onCameraChange;
    [SerializeField] private UnityEvent<GameObject, GameObject> _objEvent;
    
    private float _verticalInput;
    private float _horizontalInput;
    private float _brakeInput;
    private Vector3 _speedVector;
    private bool _isHandBraking;
    private float _curBrakeForce;
    private float _steerAngle;
    private float _fuelAmount = 100;
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
        _fuelScale.localScale = new Vector3(_fuelAmount / 100, 1, 1);
        var speedRot = _speedometerArrow.localRotation.eulerAngles;
        speedRot = new Vector3(speedRot.x, speedRot.y, Mathf.Clamp(-_rb.velocity.magnitude * 1.5f, -210, 0));
        _speedometerArrow.localRotation = Quaternion.Euler(speedRot);
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
            GetOut();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            _onCameraChange?.Invoke();
        }
    }

    private void HandleMotor()
    {
        _verticalInput = _fuelAmount > 0 ? _verticalInput : 0;
        
        switch (_wheelDriveType)
        {
            case WheelDriveType.ForwardWheelDrive:
                _frontLeftWheelCollider.motorTorque = 2 * _verticalInput * _motorForce;
                _frontRightWheelCollider.motorTorque = 2 * _verticalInput * _motorForce;
                _fuelAmount -= Mathf.Abs(_frontLeftWheelCollider.motorTorque) * 0.0001f * _fuelUsageMultiplier * Time.deltaTime;
                break;
            case WheelDriveType.RearWheelDrive:
                _rearLeftWheelCollider.motorTorque = 2 * _verticalInput * _motorForce;
                _rearRightWheelCollider.motorTorque = 2 * _verticalInput * _motorForce;
                _fuelAmount -= Mathf.Abs(_rearLeftWheelCollider.motorTorque) * 0.0001f * _fuelUsageMultiplier * Time.deltaTime;
                break;
            case WheelDriveType.FullWheelDrive:
                _frontLeftWheelCollider.motorTorque = _verticalInput * _motorForce;
                _frontRightWheelCollider.motorTorque = _verticalInput * _motorForce;
                _rearLeftWheelCollider.motorTorque = _verticalInput * _motorForce;
                _rearRightWheelCollider.motorTorque = _verticalInput * _motorForce;
                _fuelAmount -= Mathf.Abs(_frontLeftWheelCollider.motorTorque) * 0.0002f * _fuelUsageMultiplier * Time.deltaTime;
                break;
        }

        _fuelAmount -= Mathf.Abs(_frontLeftWheelCollider.motorTorque) * 0.0001f * _fuelUsageMultiplier * Time.deltaTime;
        Debug.Log(_frontLeftWheelCollider.motorTorque);
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
        rotAngle = Mathf.LerpAngle(rotAngle, -10 * _steerAngle, 0.5f);
        _steeringWheel.localRotation = Quaternion.Euler(new Vector3(curRot.x, curRot.y, rotAngle)); 
    }
    
    private void GetOut()
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
