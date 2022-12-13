using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewController : MonoBehaviour
{
    [SerializeField] private float _changeSpeed = 1.0f;
    [SerializeField] private float _defauitFOV = 90.0f, _aimFOV = 65.0f, _addShiftFov = 20.0f;
    [SerializeField] private Camera _camera;

    private float FOVTarget;
    private float AddFOVShift;

    private void Awake()
    {
        FOVTarget = _defauitFOV;
        AddFOVShift = 0;

        InputHandler.OnRightMouseDown += () => { FOVTarget = _aimFOV; };
        InputHandler.OnRightMouseUp += () => { FOVTarget = _defauitFOV; };
        InputHandler.OnPressShift += () => { AddFOVShift = HeroState.IsIdleAim ? _addShiftFov : 0; };
        InputHandler.OnShiftUp += () => { AddFOVShift = 0; };
    }

    private void FixedUpdate()
    {
        _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, FOVTarget - AddFOVShift, Time.deltaTime * _changeSpeed);
    }

    private void OnEnable() { }
    private void OnDisable() { }
    private void OnDestroy() { }
}
