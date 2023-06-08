using System;
using System.Collections;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace UI
{
    public class PopupController : MonoBehaviour
    {
        [field: SerializeField] public GameObject Popup { get; private set; }

        [field: SerializeField] public string[] Lines { get; private set; }

        [field: SerializeField] public float Delay { get; private set; } = 0.3f;

        [field: SerializeField] public TMP_Text Text { get; private set; }

        private float previousTimeScale;

        private void Awake()
        {
            Popup.SetActive(false);
            //Text.SetText("");
        }

        public void Show()
        {
            if (Time.timeScale == 0)
                return;

            // StartCoroutine(PrintText());
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Popup.SetActive(true);
        }

        private IEnumerator PrintText()
        {
            var builder = new StringBuilder();

            foreach (var line in Lines)
            {
                builder.AppendLine(line);
                builder.AppendLine();

                Text.SetText(builder.ToString());

                yield return new WaitForSecondsRealtime(Delay);
            }
        }

        public void Hide()
        {
            Time.timeScale = previousTimeScale;
            Cursor.lockState = CursorLockMode.Locked;
            Popup.SetActive(false);
        }
    }
}