using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> _images;
    [SerializeField] private Image _background;

    private void Awake()
    {
        _background.sprite = _images[Random.Range(0, _images.Count)];
    }
}
