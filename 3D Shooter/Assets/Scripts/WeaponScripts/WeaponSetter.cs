using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponTargetLooker))]
[RequireComponent(typeof(Recoil))]
[RequireComponent(typeof(AmmoController))]
[RequireComponent(typeof(WeaponSoundsPlayer))]
[RequireComponent(typeof(CameraShaker))]

public class WeaponSetter : MonoBehaviour
{
    private Recoil recoil;
    private WeaponTargetLooker targetLooker;
    private GameObject hero;
    private AmmoController ammoController;
    private WeaponSoundsPlayer weaponAudioPlayer;
    private CameraShaker cameraShaker;

    private void Awake()
    {
        recoil = GetComponent<Recoil>();
        targetLooker = GetComponent<WeaponTargetLooker>();
        ammoController = GetComponent<AmmoController>();
        weaponAudioPlayer = GetComponent<WeaponSoundsPlayer>();
        cameraShaker = GetComponent<CameraShaker>();
        hero = GameObject.FindGameObjectWithTag("Hero");
        var cameraC = hero.GetComponent<CameraController>();

        weaponAudioPlayer.SetAdioSource(hero.GetComponent<AudioSource>());
        recoil.SetCameraController(cameraC);
        targetLooker.SetTargetLook(cameraC.GetTargetLook());
        ammoController.Init(hero.GetComponent<InventoryCreator>().GetModel());
        cameraShaker.SetCamera(cameraC.GetCamera);
    }

    private void OnEnable()
    {
        if (!recoil)
            recoil = GetComponent<Recoil>();
        Crosshair.Instance.SetRecoil(recoil);
    }
}
