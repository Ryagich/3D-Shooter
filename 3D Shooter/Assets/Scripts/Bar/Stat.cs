using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    public event Action<float> StateChanged; 

    public BarView BarV => _barV;
    public BarModel BarM { get; private set; }

    [SerializeField] private HpController hpC;
    [SerializeField] private BarView _barV;
    [SerializeField, Min(.0f)] private float _descentTime;
    [SerializeField] private float _criticalValue;
    [SerializeField] private float defPower;
    [SerializeField] private float _perDamage = -0.1f, _perDamageTime = 1.0f;
    [SerializeField, Min(.0f)] private float _maxAmount = 100.0f, _amount = 50.0f;

    private float power;

    public void Awake()
    {
        BarM = new BarModel(_amount, _maxAmount);
        BarM.AmountChanged += BarV.UpdateBar;
        BarV.UpdateBar(BarM.Amount, BarM.MaxAmount);
        power = defPower;

        Invoke("ChangeAmountPeriodically", _descentTime);
    }

    public void ChangeAmount(float value)
    {
        BarM.ChangeAmount(value);
        DealDamageOverTime();
    }

    private void ChangeAmountPeriodically()
    {
        BarM.ChangeAmount(power);
        DealDamageOverTime();
        Invoke("ChangeAmountPeriodically", _descentTime);
        StateChanged?.Invoke(power);
    }

    private void ChangePower(float newPower)
    {
        power = newPower;
    }

    private void DealDamageOverTime()
    {
        if (BarM.Amount == _criticalValue)
        {
            hpC.ChangeAmount(_perDamage);
            Invoke("DealDamageOverTime", _perDamageTime);
        }
    }
}
