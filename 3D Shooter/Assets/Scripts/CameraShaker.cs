using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private float _time, _angleDeg;
    [SerializeField] private float _power;
    [SerializeField] private Vector3 _baseRecoil;

    private new Camera camera;
    private WeaponController weaponC;
    private Recoil recoil;

    private void Awake()
    {
        weaponC = GetComponent<WeaponController>();
        recoil = GetComponent<Recoil>();
    }

    private void OnEnable()
    {
        weaponC.OnShoot += ShakeRotateCamera;
    }

    private void OnDisable()
    {
        weaponC.OnShoot -= ShakeRotateCamera;
    }

    public void SetCamera(Camera camera)
    {
        this.camera = camera;
    }

    public void ShakeRotateCamera()
    {
        StartCoroutine(ShakeRotateCor());
    }

    private IEnumerator ShakeRotateCor()
    {
        if (Random.Range(0, 100) > 50)
            _angleDeg *= -1;
        var randomRotation = new Vector3(0, 0, _angleDeg * recoil.RecoilPower) +
                             new Vector3(0, 0, Random.Range(-_angleDeg, _angleDeg) * recoil.RecoilRandomOffset);
        var randomTranslation = _baseRecoil * recoil.RecoilPower +
            new Vector3(Random.Range(-_power, _power), Random.Range(-_power, 0), Random.Range(-_power, 0)) * recoil.RecoilRandomOffset;
        var elapsed = 0f;
        var halfDuration = _time / 2;
        var startPos = Vector3.zero;
        randomTranslation += startPos;
        while (elapsed < _time)
        {
            var t = elapsed < halfDuration ? elapsed / halfDuration : (_time - elapsed) / halfDuration;
            var sign = Mathf.Sign(halfDuration - elapsed);
            camera.transform.Rotate(randomRotation * sign * t);
            transform.parent.localPosition = Vector3.Lerp(startPos, randomTranslation, t * t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.parent.localPosition = startPos;
    }
}
