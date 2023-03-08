using UnityEngine;
using UnityEngine.Events;

public class HpController : MonoBehaviour
{
    public UnityEvent OnDeath { get; } = new ();

    public bool IsAlive { get; private set; } = true;
    public BarModel BarM
    {
        get
        {
            hpM ??= new BarModel(_hp, _maxHp);
            return hpM;
        }
    }

    [SerializeField] private Fader _hpFader;
    [SerializeField] private BarView _hpV;
    [SerializeField, Min(.0f)] private float _hp = 100.0f, _maxHp = 100.0f;

    private BarModel hpM;

    private void Awake()
    {
        if (_hpV)
            BarM.AmountChanged += _hpV.UpdateBar;
        if (_hpFader)
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
            Debug.Log("����");
            if (_hpV)
                _hpV.ChangeState(false);
            IsAlive = false;
            OnDeath?.Invoke();
            return;
        }
        if (_hpV)
            _hpV.ChangeState(!BarM.IsMax);
    }
}
