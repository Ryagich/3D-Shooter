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

    public static event Action OnMouseDown;
    public static event Action OnMouseUp;
    public static event Action OnPressSpace;
    public static event Action OnPressI;
    public static event Action OnPressR;

    private void Update()
    {
        OnMove?.Invoke(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
        OnMouse?.Invoke(Input.mousePosition);
        OnMouseX?.Invoke(Input.GetAxis("Mouse X"));
        OnMouseY?.Invoke(Input.GetAxis("Mouse Y"));

        if (Input.GetKeyDown(KeyCode.Mouse0))
            OnMouseDown?.Invoke();
        if (Input.GetKeyUp(KeyCode.Mouse0))
            OnMouseUp?.Invoke();

        if (Input.GetKeyDown(KeyCode.Space))
            OnPressSpace?.Invoke();
        if (Input.GetKeyDown(KeyCode.I))
            OnPressI?.Invoke();
        if (Input.GetKeyDown(KeyCode.R))
            OnPressR?.Invoke();
    }
}
