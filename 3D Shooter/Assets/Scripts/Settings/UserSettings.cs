using UnityEngine;

namespace Settings
{
    public static class UserSettings
    {
        public static float CameraSensitivity { get; set; } = 100;

        private const string CameraSensitivityPref = "CameraSensitivity";

        public static void Load()
        {
            CameraSensitivity = PlayerPrefs.GetFloat(CameraSensitivityPref, 100);
        }

        public static void Save()
        {
            PlayerPrefs.SetFloat(CameraSensitivityPref, CameraSensitivity);
        }
    }
}