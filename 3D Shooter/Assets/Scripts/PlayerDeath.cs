using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private List<GameObject> _interfaces;
    [SerializeField] private GameObject _deathScreen;

    private Hp hp;

    private void Awake()
    {
        HeroState.IsDead = false;
        hp = GetComponent<Hp>();
        hp.OnDead += Die;
    }

    private void Die()
    {
        Cursor.lockState = CursorLockMode.Confined;
        foreach (var i in _interfaces)
            i.SetActive(false);
        _deathScreen.SetActive(true);
        HeroState.IsDead = true;
    }
}
