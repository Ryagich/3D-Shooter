using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hp : MonoBehaviour
{
    public event Action OnDead;

    [SerializeField, Min(0.0f)] private float _maxHp = 100.0f, _hp = 100.0f;

    public void TakeDamage(float damage)
    {
        _hp = Mathf.Max(0, _hp - damage);
        if (_hp <= 0) 
        {
            Death();
        }
    }

    public void TakeHp(float health)
    {
        _hp = Mathf.Min(_maxHp, _hp + health);
    }

    public void Death()
    {
        Destroy(gameObject);
        OnDead.Invoke();
    }
}
