using System;
using System.Collections;
using UnityEngine;

public class InterfaceController : MonoBehaviour
{
    [SerializeField, Min(.0f)] private float _startTime = 5.0f, _defHoldTime = 10.0f, _endTime = 5f;
    [SerializeField] private Fader[] _faders;

    private void Awake()
    {
        foreach (var fader in _faders)
            fader.SetDigits(_startTime, _defHoldTime, _endTime);

        InputHandler.TabDowned += Show;
        InputHandler.TabUped += Hide;
    }

    private void Show()
    {
        foreach (var fader in _faders)
            fader.Show(false);
    }

    private void Hide()
    {
        var maxTime = float.MinValue;

        foreach (var fader in _faders)
        {
            var lastStartTime = fader.GetLastStartTime();
            if (maxTime < lastStartTime)
                maxTime = lastStartTime;
        }
        foreach (var fader in _faders)
            if (fader.State == FaderState.start)
            {
                var addTime = maxTime - fader.GetLastStartTime();
                fader.Show(true, addTime);
            }
            else
                fader.Hide(maxTime);
    }
}
