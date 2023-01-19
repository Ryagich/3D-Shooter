using System;
using System.Collections.Generic;
using UnityEngine;

public class HpController : MonoBehaviour
{
    public event Action OnDead;

    public bool IsAlive { get; private set; } = true;
    public BarModel HpM
    {
        get
        {
            if (hpM == null)
                hpM = new BarModel(_hp, _maxHp);
            return hpM;
        }
    }


    [SerializeField, Min(0.0f)] private float _hp = 100.0f, _maxHp = 100.0f;
    [SerializeField] private BarView hpV;

    private BarModel hpM;

    private void Awake()
    {
        HpM.OnAmountChanged += hpV.UpdateBar;
    }

    public void ChangeHp(float value)
    {
        if (!IsAlive)
            return;
        HpM.ChangeAmount(value);

        if (HpM.Amount == 0)
        {
            hpV.ChangeState(false);
            IsAlive = false;
            OnDead?.Invoke();
            return;
        }

        if (HpM.IsMax)
            hpV.ChangeState(false);
        else
            hpV.ChangeState(true);
    }
}
