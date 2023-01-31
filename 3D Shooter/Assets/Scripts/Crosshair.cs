using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public static Crosshair Instance;

    [SerializeField] private float _power = 1.0f;
    [SerializeField] private List<Image> _signalImages;
    [SerializeField, Range(.0f, 1.0f)] private float _showSpeed = 0.5f, _hideSpeed = 0.2f, _hideTime = .5f;
    [SerializeField] private Recoil _recoil;

    private RectTransform crosshairTrans;
    private float t, targetT;

    private void Awake()
    {
        Instance = this;
        crosshairTrans = GetComponent<RectTransform>();

        InputHandler.RightMouseDowned += () => { gameObject.SetActive(!HeroState.IsWeaponOnHand); };
        InputHandler.RightMouseUped += () => { gameObject.SetActive(true); };
    }

    public void SetRecoil(Recoil recoil)
    {
        if (_recoil)
            recoil.OnHeatChanged -= RecoilUpdate;
        _recoil = recoil;
        _recoil.OnHeatChanged += RecoilUpdate;
    }

    private void RecoilUpdate(float heat)
    {
        crosshairTrans.sizeDelta = new Vector2(heat, heat) * _power;
    }

    public void ShowSignalCrosshair()
    {
        targetT = 1;
    }

    private void ApplyT(float t)
    {
        foreach (var image in _signalImages)
        {
            image.color = image.color.WithA(255 * t);
            image.transform.localScale = Vector3.one * t;
        }
    }

    private void Update()
    {
        targetT = Mathf.MoveTowards(targetT, 0, Time.deltaTime * _hideTime);
        var holdedT = targetT > _hideTime ? 1 : targetT;
        t = Mathf.Lerp(t, holdedT, t < targetT ? _showSpeed : _hideSpeed);
        ApplyT(t);
    }
}
