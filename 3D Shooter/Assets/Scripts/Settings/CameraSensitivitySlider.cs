using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
    [RequireComponent(typeof(Slider))]
    public class CameraSensitivitySlider : MonoBehaviour
    {
        private Slider slider;
        private const int Modifier = 20;

        private void Awake()
        {
            slider = GetComponent<Slider>();
            slider.onValueChanged.AddListener(value =>  UserSettings.CameraSensitivity = value * Modifier);
        }

        private void OnEnable()
        {
            slider.value = UserSettings.CameraSensitivity / Modifier;
        }
    }
}