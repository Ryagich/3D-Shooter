using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroState : MonoBehaviour
{
    public static bool IsReload = false;
    public static bool IsInventory = false;
    public static bool IsDead = false;
    public static bool IsWeaponOnHand = false;
    public static bool isShooting;
    public static bool IsIdleAim => InputHandler.IsRightMouse && InputHandler.IsShift;


    private void Awake()
    {
        ChangeHand();

        GetComponent<WorldHandItemController>().OnChangeHandItem += ChangeHand;
    }

    private void ChangeHand(HandItem hand = null)
    {
        IsWeaponOnHand = hand && hand.GetComponent<WeaponController>();
    }
}
