using UnityEngine;

namespace Settings
{
    public static class UserSettings
    {
        public static float CameraSensitivity { get; set; } = 10;

        private const string CameraSensitivityPref = "CameraSensitivity";

        public static void Load()
        {
# if DEBUG
            CameraSensitivity = 100;
# else
            CameraSensitivity = PlayerPrefs.GetFloat(CameraSensitivityPref, 10);
#endif
        }

        public static void Save()
        {
            PlayerPrefs.SetFloat(CameraSensitivityPref, CameraSensitivity);
        }
    }
}