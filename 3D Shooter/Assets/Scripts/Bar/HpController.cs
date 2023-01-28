using System;
using System.Collections.Generic;
using UnityEngine;

public class HpController : MonoBehaviour
{
    public event Action Deaded;

    public bool IsAlive { get; private set; } = true;
    public BarModel BarM
    {
        get
        {
            if (hpM == null)
                hpM = new BarModel(_hp, _maxHp);
            return hpM;
        }
    }

    [SerializeField] private Fader _hpFader;
    [SerializeField] private BarView _hpV;
    [SerializeField, Min(0.0f)] private float _hp = 100.0f, _maxHp = 100.0f;

    private BarModel hpM;

    private void Awake()
    {
        BarM.AmountChanged += _hpV.UpdateBar;
        BarM.AmountChanged += (_, _) => _hpFader.Show(true);
    }

    public void ChangeAmount(float value)
    {
        Debug.Log(value);

        if (!IsAlive)
            return;
        BarM.ChangeAmount(value);
        Debug.Log(BarM.Amount);
        if (BarM.Amount == 0)
        {
            Debug.Log("Умер");
            _hpV.ChangeState(false);
            IsAlive = false;
            Deaded?.Invoke();
            return;
        }
        _hpV.ChangeState(!BarM.IsMax);
    }
}
