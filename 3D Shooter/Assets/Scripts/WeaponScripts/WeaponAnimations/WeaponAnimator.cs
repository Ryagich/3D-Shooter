using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WeaponAnimator : MonoBehaviour
{
    private Animator animator;
    private WeaponController weaponC;
    private AmmoController ammoC;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        weaponC = GetComponent<WeaponController>();
        ammoC = GetComponent<AmmoController>();
    }

    private void FixedUpdate()
    {
        animator.SetBool("IsAim", InputHandler.IsRightMouse);
        animator.SetBool("IsIdleAim", HeroState.IsIdleAim);
        animator.SetBool("IsShooting", weaponC.IsShooting);
    }

    private void SetReload()
    {
        if (!ammoC.HasFullMagazine && ammoC.GetTotalCount() > 0)
            animator.SetTrigger("Reload");
    }

    private void OnEnable()
    {
        InputHandler.OnRDown += SetReload;
    }

    private void OnDisable()
    {
        InputHandler.OnRDown -= SetReload;
    }

    private void OnDestroy()
    {
        InputHandler.OnRDown -= SetReload;
    }
}
