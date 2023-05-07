using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointerIcon : MonoBehaviour
{
    [SerializeField] private Image _image;
    private bool isShown;

    private void Awake()
    {
        _image.enabled = false;
        isShown = false;
    }

    public void SetIconPosition(Vector3 position, Quaternion rotation)
    => transform.SetPositionAndRotation(position, rotation);

    public void Show()
    {
        if (isShown) return;
        isShown = true;
        StopAllCoroutines();
        StartCoroutine(ShowProcess());
    }

    public void Hide()
    {
        if (!isShown) return;
        isShown = false;

        StopAllCoroutines();
        StartCoroutine(HideProcess());
    }

    private IEnumerator ShowProcess()
    {
        _image.enabled = true;
        transform.localScale = Vector3.zero;
        for (float t = 0; t < 1f; t += Time.deltaTime * 4f)
        {
            transform.localScale = Vector3.one * t;
            yield return null;
        }
        transform.localScale = Vector3.one;
    }

    private IEnumerator HideProcess()
    {
        for (float t = 0; t < 1f; t += Time.deltaTime * 4f)
        {
            transform.localScale = Vector3.one * (1f - t);
            yield return null;
        }
        _image.enabled = false;
    }
}
