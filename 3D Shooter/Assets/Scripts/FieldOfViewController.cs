using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewController : MonoBehaviour
{
    [SerializeField] private float _changeSpeed = 1.0f;
    [SerializeField] private float _defauitFOV = 90.0f, _aimFOV = 65.0f, _addShiftFov = 20.0f;
    [SerializeField] private Camera _camera;
    
    private float FOVTarget;
    private float addFOVShift;

    private void Awake()
    {
        FOVTarget = _defauitFOV;
        addFOVShift = 0;

        InputHandler.RightMouseDowned += () => { FOVTarget = _aimFOV; };
        InputHandler.RightMouseUped += () => { FOVTarget = _defauitFOV; };
        InputHandler.ShiftPressed += () => { addFOVShift = HeroState.IsIdleAim ? _addShiftFov : 0; };
        InputHandler.ShiftUped += () => { addFOVShift = 0; };
    }

    private void FixedUpdate()
    {
        _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView,
                                         HeroState.IsWeaponOnHand ?
                                         FOVTarget - addFOVShift :
                                         _defauitFOV,
                                         Time.fixedDeltaTime * _changeSpeed);
    }
}
