using UnityEngine.SceneManagement;

namespace Settings
{
    public class SettingsStartup : MonoBehaviourSingleton<SettingsStartup>
    {
        protected override void SingletonAwakened()
        {
            SceneManager.sceneLoaded += (_, _) => UserSettings.Load();
        }
    }
}