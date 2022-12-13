using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    public event Action<float> OnHeatChanged;

    public float Heat { get; private set; }
    public float RecoilPower { get; private set; }
    public float RecoilRandomOffset { get; private set; }

    [SerializeField] private Vector2 _recoilPower;
    [SerializeField] private AnimationCurve _recoilRandomOffsetCurve;
    [SerializeField] private AnimationCurve _recoilCurve;
    [SerializeField, Range(0.0f, 1.0f)] private float _coolSpeed;
    [SerializeField, Range(0.0f, 1.0f)] private float _smooth;

    private Vector2 accum;
    private CameraController cameraC;
    private WeaponController weaponC;

    private void Awake()
    {
        weaponC = GetComponent<WeaponController>();
    }

    public void SetCameraController(CameraController controller)
    {
        cameraC = controller;
    }

    private void OnShoot()
    {
        Heat += 1;
        ApplyRecoil();
    }

    private void ApplyRecoil()
    {
        RecoilPower = _recoilCurve.Evaluate(Heat);
        RecoilRandomOffset = _recoilRandomOffsetCurve.Evaluate(Heat);

        var power = RecoilPower * new Vector2(UnityEngine.Random.Range(-_recoilPower.x, _recoilPower.x), UnityEngine.Random.Range(0, _recoilPower.y));
        var random = new Vector2(UnityEngine.Random.Range(-power.x, power.x),
                                 UnityEngine.Random.Range(0, power.y)) * RecoilRandomOffset;

        accum += power + random;
    }

    private void FixedUpdate()
    {
        Heat = Mathf.Lerp(Heat, 0, _coolSpeed);

        var move = accum * (1 - _smooth);
        accum -= move;

        cameraC.RotateX(move.x);
        cameraC.RotateY(move.y);

        OnHeatChanged?.Invoke(Heat);
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
}
