using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBarStateChanger : MonoBehaviour
{
    [SerializeField] private BarView _hpV;

    private HpController hpC;

    private void Awake()
    {
        hpC = GetComponent<HpController>();
        hpC.HpM.OnAmountChanged += ChangeBarState;
        ChangeBarState();
    }

    private void ChangeBarState(float _ = 0.0f, float __ = 0.0f)
    {
        _hpV.ChangeState(hpC.IsAlive && !hpC.HpM.IsMax);
    }
}
