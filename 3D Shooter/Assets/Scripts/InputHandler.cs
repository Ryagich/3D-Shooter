using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static event Action<Vector3> OnMouse;
    public static event Action<float> OnMouseX;
    public static event Action<float> OnMouseY;

    public static event Action LeftMouseDowned;
    public static event Action LeftMouseUped;
    public static event Action LeftMousePressed;
    public static event Action RightMouseDowned;
    public static event Action RightMouseUped;
    public static event Action SpacePressed;
    public static event Action IDowned;
    public static event Action RDowned;
    public static event Action FDowned;
    public static event Action ShiftDowned;
    public static event Action ShiftUped;
    public static event Action ShiftPressed;
    public static event Action FirstWeaponChoosed;
    public static event Action SecondWeaponChoosed;
    public static event Action TrirdWeaponChoosed;
    public static event Action VDowned;
    public static event Action TabDowned;
    public static event Action TabUped;

    public static Vector3 Movement;

    public static bool IsLeftMouse { get; private set; } = false;
    public static bool IsRightMouse { get; private set; } = false;
    public static bool IsShift { get; private set; } = false;
    public static Vector2 MousePos => Input.mousePosition;

    private void Update()
    {
        if (HeroState.IsDead)
            return;
        Movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        OnMouse?.Invoke(Input.mousePosition);
        OnMouseX?.Invoke(Input.GetAxis("Mouse X"));
        OnMouseY?.Invoke(Input.GetAxis("Mouse Y"));

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            IsLeftMouse = true;
            LeftMouseDowned?.Invoke();
        }
        if (Input.GetKey(KeyCode.Mouse0))
            LeftMousePressed?.Invoke();
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            IsLeftMouse = false;
            LeftMouseUped?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            IsRightMouse = true;
            RightMouseDowned?.Invoke();
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            IsRightMouse = false;
            RightMouseUped?.Invoke();
        }

        if (Input.GetKey(KeyCode.Space))
            SpacePressed?.Invoke();
        if (Input.GetKeyDown(KeyCode.I))
            IDowned?.Invoke();
        if (Input.GetKeyDown(KeyCode.R))
            RDowned?.Invoke();
        if (Input.GetKeyDown(KeyCode.F))
            FDowned?.Invoke();
        if (Input.GetKey(KeyCode.LeftShift))
            ShiftPressed?.Invoke();
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            IsShift = true;
            ShiftDowned?.Invoke();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            IsShift = false;
            ShiftUped?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.V))
            VDowned?.Invoke();

        if (Input.GetKeyDown(KeyCode.Alpha1))
            FirstWeaponChoosed?.Invoke();
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SecondWeaponChoosed?.Invoke();
        if (Input.GetKeyDown(KeyCode.Alpha3))
            TrirdWeaponChoosed?.Invoke();

        if (Input.GetKeyDown(KeyCode.Tab))
            TabDowned?.Invoke();
        if (Input.GetKeyUp(KeyCode.Tab))
            TabUped?.Invoke();
    }
}
