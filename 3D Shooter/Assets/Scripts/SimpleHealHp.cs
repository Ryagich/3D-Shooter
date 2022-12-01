using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleHealHp : MonoBehaviour
{
    [SerializeField] private float _heal = 1, _coolDown = 1.0f;
    private Hp hp;

    private void Awake()
    {
        hp = GetComponent<Hp>();
        Heal();
    }

    private void Heal()
    {
        if (hp)
            hp.ChangeHp(_heal);
        Invoke(nameof(Heal), _coolDown);
    }
}
