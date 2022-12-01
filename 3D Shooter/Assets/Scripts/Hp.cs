using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hp : MonoBehaviour
{
    public event Action<float, float> OnHpChanged;
    public event Action<float> OnHpChangedValue;
    public event Action OnDead;

    [SerializeField, Min(0.0f)] private float _maxHp = 100.0f, _hp = 100.0f;

    public float MaxHP { get => _maxHp; }
    public float HP { get => _hp; }

    public void ChangeHp(float value)
    {
        _hp = Mathf.Clamp(_hp + value, 0, _maxHp);
        OnHpChanged?.Invoke(_hp, _maxHp);
        OnHpChangedValue?.Invoke(value);

        if (_hp <= 0)
            OnDead?.Invoke();
    }
}
