using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static event Action<Vector3> OnMove;
    public static event Action<Vector3> OnMouse;
    public static event Action<float> OnMouseX;
    public static event Action<float> OnMouseY;

    public static event Action OnLeftMouseDown;
    public static event Action OnLeftMouseUp;
    public static event Action OnLeftMouse;
    public static event Action OnRightMouseDown;
    public static event Action OnRightMouseUp;
    public static event Action OnPressSpace;
    public static event Action OnIDown;
    public static event Action OnRDown;
    public static event Action OnFDown;
    public static event Action OnShiftDown;
    public static event Action OnShiftUp;
    public static event Action OnPressShift;
    public static event Action OnFirstWeapon;
    public static event Action OnSecondWeapon;
    public static event Action OnTrirdWeapon;
    public static event Action OnVDown;
    public static event Action OnTabDown;
    public static event Action OnTabUp;

    public static bool IsLeftMouse { get; private set; } = false;
    public static bool IsRightMouse { get; private set; } = false;
    public static bool IsShift { get; private set; } = false;
    public static Vector2 MousePos => Input.mousePosition;

    private void Update()
    {
        if (HeroState.IsDead)
            return;
        OnMove?.Invoke(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
        OnMouse?.Invoke(Input.mousePosition);
        OnMouseX?.Invoke(Input.GetAxis("Mouse X"));
        OnMouseY?.Invoke(Input.GetAxis("Mouse Y"));

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            IsLeftMouse = true;
            OnLeftMouseDown?.Invoke();
        }
        if (Input.GetKey(KeyCode.Mouse0))
            OnLeftMouse?.Invoke();
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            IsLeftMouse = false;
            OnLeftMouseUp?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            IsRightMouse = true;
            OnRightMouseDown?.Invoke();
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            IsRightMouse = false;
            OnRightMouseUp?.Invoke();
        }

        if (Input.GetKey(KeyCode.Space))
            OnPressSpace?.Invoke();
        if (Input.GetKeyDown(KeyCode.I))
            OnIDown?.Invoke();
        if (Input.GetKeyDown(KeyCode.R))
            OnRDown?.Invoke();
        if (Input.GetKeyDown(KeyCode.F))
            OnFDown?.Invoke();
        if (Input.GetKey(KeyCode.LeftShift))
            OnPressShift?.Invoke();
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            IsShift = true;
            OnShiftDown?.Invoke();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            IsShift = false;
            OnShiftUp?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.V))
            OnVDown?.Invoke();

        if (Input.GetKeyDown(KeyCode.Alpha1))
            OnFirstWeapon?.Invoke();
        if (Input.GetKeyDown(KeyCode.Alpha2))
            OnSecondWeapon?.Invoke();
        if (Input.GetKeyDown(KeyCode.Alpha3))
            OnTrirdWeapon?.Invoke();

        if (Input.GetKeyDown(KeyCode.Tab))
            OnTabDown?.Invoke();
        if (Input.GetKeyUp(KeyCode.Tab))
            OnTabUp?.Invoke();
    }
}
