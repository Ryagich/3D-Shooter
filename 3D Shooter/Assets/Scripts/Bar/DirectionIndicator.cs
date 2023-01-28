using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectionIndicator : MonoBehaviour
{
    [SerializeField] private Stat _stat;
    [SerializeField] private Image _signalImage;

    private void Awake()
    {
        _stat.StateChanged += ChangeImageState;
    }

    private void ChangeImageState(float value)
    {
        _signalImage.gameObject.SetActive(value != .0f);
        _signalImage.gameObject.transform.rotation = new Quaternion(0, 0,
        value > 0 ? 0 : 180, _signalImage.gameObject.transform.rotation.w);
    }
}
