using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DamageOverlayController : MonoBehaviour
    {
        [field: SerializeField] public float DamageThreshold { get; private set; } = 5;
        [field: SerializeField] [field: Range(0, 255)] public int Strength { get; private set; } = 140;
        [field: SerializeField] [field: Min(0)] public float FadeDurationSeconds { get; private set; } = 1;

        [field: SerializeField] public HpController HpController { get; private set; }
        [field: SerializeField] public Image Image { get; private set; }
        
        private Coroutine activeCoroutine;

        private void Awake()
        {
            HpController.OnAmountChanged.AddListener(delta =>
            {
                if (-delta < DamageThreshold)
                    return;

                if (activeCoroutine != null)
                    StopCoroutine(activeCoroutine);

                Image.color = new Color(Image.color.r, Image.color.g, Image.color.b, Strength / 255f);
                activeCoroutine = StartCoroutine(FadeOverlay());
            });
        }

        private IEnumerator FadeOverlay()
        {
            var color = Image.color;
            var initialA = color.a;

            var fadeTimer = 0f;
            while (fadeTimer <= FadeDurationSeconds)
            {
                var alpha = Mathf.Lerp(initialA, 0, fadeTimer / FadeDurationSeconds);

                color.a = alpha;
                Image.color = color;

                fadeTimer += Time.deltaTime;

                yield return null;
            }

            color.a = 0;
            Image.color = color;
        }
    }
}