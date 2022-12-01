using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSetter : MonoBehaviour
{
    private Recoil recoil;
    private WeaponAnimator animator;
    private GameObject hero;
    private AmmoController ammoController;
    private WeaponSoundsPlayer weaponAudioPlayer;

    private void Awake()
    {
        recoil = GetComponent<Recoil>();
        animator = GetComponent<WeaponAnimator>();
        ammoController = GetComponent<AmmoController>();
        weaponAudioPlayer = GetComponent<WeaponSoundsPlayer>();
        hero = GameObject.FindGameObjectWithTag("Hero");
        var cameraC = hero.GetComponent<CameraController>();

        weaponAudioPlayer.SetAdioSource(hero.GetComponent<AudioSource>());
        recoil.SetCameraController(cameraC);
        animator.SetTargetLook(cameraC.GetTargetLook());
        ammoController.Init(hero.GetComponent<InventoryController>().Model);
    }

    private void OnEnable()
    {
        if (!recoil)
            recoil = GetComponent<Recoil>();
        Crosshair.Instance.SetRecoil(recoil);
    }
}
