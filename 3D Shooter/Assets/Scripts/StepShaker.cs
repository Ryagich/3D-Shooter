using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepShaker : MonoBehaviour
{
    [SerializeField] private float _stepDistance = 5.0f;

    [Header("Shake Stats")]
    [SerializeField, Min(0.0f)] private float _time = 1.0f, _stepAngle = 2.0f, _sideAngle = 5.0f, _treshold = 0.05f;

    private CameraShaker shaker;
    private bool isRotate;

    private void Awake()
    {
        shaker = GetComponent<CameraShaker>(); 

        InputHandler.OnMove += TurnCamera;
    }

    private void TurnCamera(Vector3 move)
    {
    }
}
