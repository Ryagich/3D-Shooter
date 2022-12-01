using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    public event Action<float> OnHeatChanged; 

    [SerializeField] private Vector2 _recoilPower;
    [SerializeField] private AnimationCurve _curve;
    [SerializeField, Range(0.0f, 1.0f)] private float _coolSpeed;
    [SerializeField, Range(0.0f, 1.0f)] private float _smooth;
    private CameraController cameraC;

    private float heat;
    private Vector2 accum;
    private WeaponController weaponC;

    private void Awake()
    {
        weaponC =  GetComponent<WeaponController>();
    }

    private void OnEnable()
    {
        weaponC.OnShoot += OnShoot;
    }

    private void OnDisable()
    {
        weaponC.OnShoot -= OnShoot;
    }

    private void OnDestroy()
    {
        weaponC.OnShoot -= OnShoot;
    }

    public void SetCameraController(CameraController controller)
    {
        cameraC = controller;
    }

    private void OnShoot()
    {
        heat += 1;
        ApplyRecoil();
    }

    private void ApplyRecoil()
    {
        var power = _curve.Evaluate(heat) * _recoilPower;

        accum.x += UnityEngine.Random.Range(-power.x, power.x);
        accum.y += UnityEngine.Random.Range(0, power.y);
    }

    private void FixedUpdate()
    {
        heat = Mathf.Lerp(heat, 0, _coolSpeed);

        var move = accum * (1 - _smooth);
        accum -= move;

        cameraC.RotateX(move.x);
        cameraC.RotateY(move.y);

        OnHeatChanged?.Invoke(heat);
    }
}
