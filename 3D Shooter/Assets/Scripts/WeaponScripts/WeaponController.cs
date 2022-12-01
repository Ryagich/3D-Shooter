using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public event Action OnShoot;
    public event Action<ShootState> OnChangeState;

    public ShootState GetCurrState => shootState;

    [SerializeField] private ParticleSystem _muzzleFlash;
    [SerializeField] private bool _single = true;
    [SerializeField] private bool _burst = true;
    [SerializeField] private bool _automate = true;
    [SerializeField, Min(0.0f)] private float _cooldownTime = 0.5f;

    private bool isReady = true;
    private ShootState shootState = ShootState.Single;
    private WeaponAnimator weaponAnimator;
    private AmmoController ammoController;
    private HandItem handItem;
    private int burstCount = 0;

    private void Awake()
    {
        weaponAnimator = GetComponent<WeaponAnimator>();
        ammoController = GetComponent<AmmoController>();
        handItem = GetComponent<HandItem>();
    }

    private void OnEnable()
    {
        InputHandler.OnLeftMouseDown += Shoot;
        InputHandler.OnRDown += Reload;
        InputHandler.OnVDown += ChangeShootState;

        OnChangeState?.Invoke(shootState);
    }

    private void OnDisable()
    {
        InputHandler.OnLeftMouseDown -= Shoot;
        InputHandler.OnRDown -= Reload;
        InputHandler.OnVDown -= ChangeShootState;
    }

    private void OnDestroy()
    {
        InputHandler.OnLeftMouseDown -= Shoot;
        InputHandler.OnRDown -= Reload;
        InputHandler.OnVDown -= ChangeShootState;
    }

    private void ChangeShootState()
    {
        if (shootState == ShootState.Single)
        {
            if (_burst)
                shootState = ShootState.Burst;
            else if (_automate)
                shootState = ShootState.Automate;
        }
        else if (shootState == ShootState.Burst)
        {
            if (_automate)
                shootState = ShootState.Automate;
            else if (_single)
                shootState = ShootState.Single;
        }
        else if (shootState == ShootState.Automate)
        {
            if (_single)
                shootState = ShootState.Single;
            else if (_burst)
                shootState = ShootState.Burst;
        }
        OnChangeState?.Invoke(shootState);
    }

    public void Shoot()
    {
        if (!weaponAnimator.IsReloading && isReady && !InputHandler.IsInventory
         && ammoController.HasAmmo)
        {
            ammoController.SubtractAmmo();
            _muzzleFlash.Play();

            if (shootState == ShootState.Single)
            {
                isReady = false;
                CorutineHolder.Instance.StartCoroutine(SingleShootCooldown());
            }
            if (shootState == ShootState.Automate && InputHandler.IsLeftMouse)
            {
                isReady = false;
                CorutineHolder.Instance.StartCoroutine(Cooldown());
            }
            if (shootState == ShootState.Burst)
            {
                handItem.CanBeChanged = false;
                CorutineHolder.Instance.StartCoroutine(BurstShootCooldown());
                burstCount++;
            }

            OnShoot?.Invoke();
        }
    }

    private void BurstShoot()
    {
        if (weaponAnimator.IsReloading || !isReady || InputHandler.IsInventory
         || !ammoController.HasAmmo)
        {
            handItem.CanBeChanged = true;
            burstCount = 0;
            return;
        }

        isReady = false;
        ammoController.SubtractAmmo();
        _muzzleFlash.Play();
        CorutineHolder.Instance.StartCoroutine(BurstShootCooldown());

        OnShoot?.Invoke();
    }

    private void Reload()
    {
        ammoController.Reload();
    }

    private IEnumerator BurstShootCooldown()
    {
        yield return new WaitForSeconds(_cooldownTime);
        burstCount++;
        isReady = true;
        if (burstCount < 4)
            BurstShoot();
        else
        {
            handItem.CanBeChanged = true;
            burstCount = 0;
        }
    }

    private IEnumerator SingleShootCooldown()
    {
        yield return new WaitForSeconds(_cooldownTime);
        isReady = true;
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(_cooldownTime);
        isReady = true;
        Shoot();
    }
}

public enum ShootState
{
    Single = 0,
    Burst = 1,
    Automate = 2
}

