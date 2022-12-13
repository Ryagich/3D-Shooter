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

    private void Awake()
    {
        animator = GetComponent<Animator>();
        weaponC = GetComponent<WeaponController>();
    }

    private void FixedUpdate()
    {
        animator.SetBool("IsAim", InputHandler.IsRightMouse);
        animator.SetBool("IsIdleAim", HeroState.IsIdleAim);
        animator.SetBool("IsShooting", weaponC.IsShooting);
    }

    private void OnEnable() { }
    private void OnDisable() { }
}
