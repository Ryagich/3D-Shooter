using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageNumbersDroper : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TMP_Text _textPref;

    private bool isAlive = true;
    private void Awake()
    {
        GetComponent<Hp>().OnHpChangedValue += InstantiateDamageNumbers;
        GetComponent<Hp>().OnDead += () => isAlive = false;
    }

    private void InstantiateDamageNumbers(float value)
    {
        if (!isAlive)
            return;
        var damageText = Instantiate(_textPref, _canvas.transform);
        damageText.transform.localPosition = Vector2.zero;
        damageText.text = value.ToString();
        damageText.color = value > 0 ? Color.green : Color.red;
    }
}
