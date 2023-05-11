using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepShaker : MonoBehaviour
{
    [SerializeField, Min(0.0f)]
    private float _time = 1.0f,
                  _stepAngle = 2.0f, _sideAngle = 5.0f, _treshold = 0.05f;

    private CharacterController characterC;
    private CameraShaker shaker;
    private bool isRotate = false, isShake = false;

    private void Awake()
    {
        characterC = GetComponent<CharacterController>();
        shaker = GetComponent<CameraShaker>();
    }

    private void FixedUpdate()
    {
        if (!characterC.isGrounded)
            return;
        if (InputHandler.Movement.magnitude > _treshold && !isShake)
            Shake();
    }

    private void Shake()
    {
        shaker.ShakeCamera(_time, _stepAngle, isRotate);
        isRotate = !isRotate;
        isShake = true;
        Invoke(nameof(ShakeCor), _time + _treshold);
    }
    private void ShakeCor() => isShake = false;
}
