using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarView : MonoBehaviour
{
    public bool IsActivity => _barObj.activeSelf;

    [SerializeField] private GameObject _barObj;
    [SerializeField] private Image _fillBar;

    public void UpdateBar(float value, float maxValue)
    {
        _fillBar.fillAmount = value / maxValue;
    }

    public void ChangeState(bool state)
    {
        if (_barObj)
            _barObj.SetActive(state);
    }
}
