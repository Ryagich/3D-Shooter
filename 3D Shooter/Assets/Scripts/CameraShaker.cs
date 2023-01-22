using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker Instance => _instance;

    [SerializeField] private Camera _camera;

    private static CameraShaker _instance;

    private void Awake()
    {
        _instance = this;
    }

    public void ShakeCamera(float time, float angleDeg, bool isRight)
    {
        if (!isRight)
            angleDeg *= -1;

        CorutineHolder.Instance.StartCoroutine(ShakeCameraCor(time, new Vector3(0, 0, angleDeg)));
    }

    public void RandomShakeCamera(float time, float angleDeg, Recoil recoil)
    {
        if (Random.Range(0, 100) > 50)
            angleDeg *= -1;
        var randomRotation = new Vector3(0, 0, angleDeg * recoil.RecoilPower + Random.Range(-angleDeg, angleDeg) * recoil.RecoilRandomOffset); //+ new Vector3(0, 0, Random.Range(-angleDeg, angleDeg) * recoil.RecoilRandomOffset);

        CorutineHolder.Instance.StartCoroutine(ShakeCameraCor(time, randomRotation));
    }

    private IEnumerator ShakeCameraCor(float time, Vector3 rotation)
    {
        var elapsed = 0f;
        var halfDuration = time / 2;

        while (elapsed < time)
        {
            var t = elapsed < halfDuration ? elapsed / halfDuration : (time - elapsed) / halfDuration;
            var sign = Mathf.Sign(halfDuration - elapsed);

            _camera.transform.Rotate(rotation * sign * t);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
