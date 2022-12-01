using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] private GameObject bar;
    [SerializeField] private Image _bar;

    private void Awake()
    {
        GetComponent<Hp>().OnHpChanged += UpdateHpBar;
        GetComponent<Hp>().OnDead += CloseBar;
    }

    private void UpdateHpBar(float hp, float maxHp)
    {
        _bar.fillAmount = hp / maxHp;
    }

    private void CloseBar()
    {
        if (bar)
        bar.SetActive(false);
    }
}
