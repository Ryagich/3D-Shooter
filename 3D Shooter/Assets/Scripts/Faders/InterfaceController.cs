using System;
using System.Collections;
using UnityEngine;

public class InterfaceController : MonoBehaviour
{
    public Fader[] Faders { get; private set; }

    [SerializeField, Min(.0f)] private float _startTime = 75.0f, _defHoldTime = 100.0f, _endTime = 75.0f;

    private void Awake()
    {
        foreach (var fader in Faders)
            fader.SetDigits(_startTime, _defHoldTime, _endTime);

        InputHandler.OnTabDown += Show;
        InputHandler.OnTabUp += Hide;
    }

    private void Show()
    {
        foreach (var fader in Faders)
            fader.Show(false);
    }

    private void Hide()
    {
        var maxTime = float.MinValue;

        foreach (var fader in Faders)
        {
            var lastStartTime = fader.GetLastStartTime();
            if (maxTime < lastStartTime)
                maxTime = lastStartTime;
        }
        foreach (var fader in Faders)
            if (fader.State == FaderState.start)
            {
                var addTime = maxTime - fader.GetLastStartTime();
                fader.Show(true, addTime);
            }
            else
                fader.Hide(maxTime);
    }
}
