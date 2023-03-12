using Settings;
using UnityEngine;

public class GameStateController : MonoBehaviourSingleton<GameStateController>
{
    private float previousTimeScale;

    protected override void SingletonAwakened()
    {
        SettingsMenuController.OnMenuActiveChange.AddListener(isActive =>
        {
            if (isActive)
            {
                previousTimeScale = Time.timeScale;
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = previousTimeScale;
            }
        });
    }
}