using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    public FaderState State { get; private set; } = FaderState.end;

    [SerializeField] private Image[] _images;

    private float startTime, endTime, holdTime, currHold = .0f;
    private bool canEnd = true;

    public void SetDigits(float startTime, float holdTime, float endTime)
    {
        this.startTime = startTime;
        this.endTime = endTime;
        this.holdTime = holdTime;
    }

    public void Show(bool canEnd)
    {
        this.canEnd = canEnd;
        currHold = Time.time + holdTime;
        State = FaderState.start;
    }

    public void Show(bool canEnd, float addHideTime)
    {
        this.canEnd = canEnd;
        currHold = Time.time + (holdTime + addHideTime);
        State = FaderState.start;
    }

    public void Hide(float addTime)
    {
        canEnd = true;
        currHold += Time.time + (holdTime + addTime);
    }

    public float GetLastStartTime() => (255 - GetAlpha()) / (255 / startTime);

    private void SetAlpha(float a)
    {
        foreach (var image in _images)
            image.color = image.color.WithA(a / 255);
    }

    private float GetAlpha() => _images[0].color.a * 255;

    private void FixedUpdate()
    {
        switch (State)
        {
            case FaderState.start:
                SetAlpha(Mathf.MoveTowards(GetAlpha(), 255, 256 / startTime * Time.fixedDeltaTime));
                if (GetAlpha() == 255)
                    State = FaderState.wait;
                break;
            case FaderState.wait:
                if (canEnd)
                    State = FaderState.hold;
                break;
            case FaderState.hold:
                if (Time.time > currHold)
                    State = FaderState.end;
                break;
            case FaderState.end:
                SetAlpha(Mathf.MoveTowards(GetAlpha(), 0, (256 / endTime) * Time.fixedDeltaTime));
                if (GetAlpha() == 0)
                {
                    State = FaderState.none;
                    currHold = .0f;
                }
                break;
        }
    }
}

public enum FaderState
{
    none = 0,
    start = 1,
    wait,
    hold,
    end
}