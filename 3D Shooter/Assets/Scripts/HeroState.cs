using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroState : MonoBehaviour
{
    public static bool IsReload = false;
    public static bool IsInventory = false;
    public static bool IsDead = false;

    public static bool isShooting;
    public static bool IsIdleAim => InputHandler.IsRightMouse && InputHandler.IsShift;
}
