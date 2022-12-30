using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public event Action OnShoot;
    public event Action<ShootState> OnChangeState;

    public ShootState GetCurrState => shootState;
    public bool IsShooting { get; private set; } = false;

    [SerializeField] private ParticleSystem _muzzleFlash;
    [SerializeField] private bool _single = true;
    [SerializeField] private bool _burst = true;
    [SerializeField] private bool _automate = true;
    [SerializeField] private int _burstCount = 3;
    [SerializeField, Min(0.0f)] private float _cooldownTime = 0.5f;

    private bool isReady = true;
    private ShootState shootState = ShootState.Single;

    private AmmoController ammoController;
    private HandItem handItem;
    private WeaponTargetLooker weaponTargetLooker;

    private void Awake()
    {
        ammoController = GetComponent<AmmoController>();
        handItem = GetComponent<HandItem>();
        weaponTargetLooker = GetComponent<WeaponTargetLooker>();
    }

    private void Shoot()
    {
        if (!HeroState.IsInventory && !HeroState.IsDead && !ammoController.IsReload)
            CorutineHolder.Instance.StartCoroutine(ShootCoroutine());
    }

    private IEnumerator ShootCoroutine()
    {
        var toShoot = 0;
        switch (shootState)
        {
            case ShootState.Single:
                toShoot = 1;
                break;
            case ShootState.Burst:
                toShoot = _burstCount;
                break;
            case ShootState.Automate:
                toShoot = int.MaxValue;
                break;
        }
        IsShooting = true;
        handItem.CanBeChanged = false;
        for (; toShoot > 0; toShoot--)
        {
            if (!CanShoot())
                break;
            if (shootState == ShootState.Automate && !InputHandler.IsLeftMouse)
                break;

            MakeShoot();
            yield return new WaitForSeconds(_cooldownTime);
            isReady = true;
        }
        handItem.CanBeChanged = true;
        IsShooting = false;
    }

    private bool CanShoot() => isReady
                                && !HeroState.IsInventory
                                && ammoController.HasAmmo;

    private void MakeShoot()
    {
        ammoController.SubtractAmmo();
        _muzzleFlash.Play();
        OnShoot?.Invoke();
        isReady = false;
    }


    private void ChangeShootState()
    {
        while (true)
        {
            shootState = EnumExtentions.Cycle(shootState);
            if (shootState == ShootState.Automate && _automate)
                break;
            if (shootState == ShootState.Single && _single)
                break;
            if (shootState == ShootState.Burst && _burst)
                break;
        }

        OnChangeState?.Invoke(shootState);
    }

    public void StartReload()
    {
        CorutineHolder.Instance.StopCoroutine(ShootCoroutine());
        ammoController.IsReload = true;
        IsShooting = false;
        handItem.CanBeChanged = false;
        isReady = false;
    }

    public void ReloadComplete()
    {
        ammoController.Reload();
        handItem.CanBeChanged = true;
        isReady = true;
    }

    private void OnEnable()
    {
        InputHandler.OnLeftMouseDown += Shoot;
        InputHandler.OnVDown += ChangeShootState;

        OnChangeState?.Invoke(shootState);
    }

    private void OnDisable()
    {
        InputHandler.OnLeftMouseDown -= Shoot;
        InputHandler.OnVDown -= ChangeShootState;
    }

    private void OnDestroy()
    {
        InputHandler.OnLeftMouseDown -= Shoot;
        InputHandler.OnVDown -= ChangeShootState;
    }
}

public enum ShootState
{
    Single = 0,
    Burst = 1,
    Automate = 2
}