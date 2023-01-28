using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropDownNumbers : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TMP_Text _textPref;

    private HpController hpC;

    private void Awake()
    {
        hpC = GetComponent<HpController>();
        hpC.BarM.AmountChanged += InstantiateDropDownNumbers;
    }

    private void InstantiateDropDownNumbers(float value, float _)
    {
        if (!hpC.IsAlive || hpC.BarM.IsMax)
            return;
        var damageText = Instantiate(_textPref, _canvas.transform);
        damageText.transform.localPosition = Vector2.zero;
        damageText.text = value.ToString();
        damageText.color = value > 0 ? Color.green : Color.red;
    }
}
