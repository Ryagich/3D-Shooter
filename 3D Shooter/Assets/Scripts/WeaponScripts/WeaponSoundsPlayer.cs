using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSoundsPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip _shotClip;

    private AudioSource audioSource;
    private WeaponController weaponC;

    private void Awake()
    {
        weaponC = GetComponent<WeaponController>();
    }

    private void OnEnable()
    {
        weaponC.OnShoot += PlayShotAudio;
    }

    private void OnDisable()
    {
        weaponC.OnShoot -= PlayShotAudio;
    }

    private void OnDestroy()
    {
        weaponC.OnShoot -= PlayShotAudio;
    }

    public void SetAdioSource(AudioSource audioSource)
    {
        this.audioSource = audioSource;
    }

    private void PlayShotAudio()
    {
        audioSource.PlayOneShot(_shotClip);
    }
}
