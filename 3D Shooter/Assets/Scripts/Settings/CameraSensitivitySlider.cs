using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
    [RequireComponent(typeof(Slider))]
    public class CameraSensitivitySlider : MonoBehaviour
    {
        private Slider slider;

        private void Awake()
        {
            slider = GetComponent<Slider>();
            slider.onValueChanged.AddListener(value =>  UserSettings.CameraSensitivity = value * 100);
        }

        private void OnEnable()
        {
            slider.value = UserSettings.CameraSensitivity / 100;
        }
    }
}