using UnityEngine;

namespace Settings
{
    public static class UserSettings
    {
        public static float CameraSensitivity { get; set; } = 10;

        private const string CameraSensitivityPref = "CameraSensitivity";

        public static void Load()
        {
            CameraSensitivity = PlayerPrefs.GetFloat(CameraSensitivityPref, 10);
        }

        public static void Save()
        {
            PlayerPrefs.SetFloat(CameraSensitivityPref, CameraSensitivity);
        }
    }
}