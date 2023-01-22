using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShaker : MonoBehaviour
{
    [SerializeField] private float _time, _angleDeg;
    [SerializeField] private float _power;
    [SerializeField] private Vector3 _baseRecoil;

    private WeaponController weaponC;
    private Recoil recoil;

    private void Awake()
    {
        weaponC = GetComponent<WeaponController>();
        recoil = GetComponent<Recoil>();
    }

    public void ShakeRotateCamera()
    {
        CorutineHolder.Instance.StartCoroutine(ShakeRotateCor());
        CameraShaker.Instance.RandomShakeCamera(_time, _angleDeg, recoil);
       //не забыть вызвать в другом месте
    }

    private IEnumerator ShakeRotateCor()
    {
        var randomTranslation = _baseRecoil * recoil.RecoilPower +
            new Vector3(Random.Range(-_power, _power), Random.Range(-_power, 0), Random.Range(-_power, 0)) * recoil.RecoilRandomOffset;
        var elapsed = 0f;
        var halfDuration = _time / 2;
        var startPos = Vector3.zero;
        randomTranslation += startPos;
        while (elapsed < _time)
        {
            var t = elapsed < halfDuration ? elapsed / halfDuration : (_time - elapsed) / halfDuration;
            transform.parent.localPosition = Vector3.Lerp(startPos, randomTranslation, t * t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.parent.localPosition = startPos;
    }

    private void OnEnable()
    {
        weaponC.OnShoot += ShakeRotateCamera;
    }

    private void OnDisable()
    {
        weaponC.OnShoot -= ShakeRotateCamera;
    }
}
