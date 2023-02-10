using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    [SerializeField] private Transform _shootPoint;

    [Header("Bullet Stats")]
    [SerializeField, Min(1.0f)] private float _speed = 1.0f;
    [SerializeField, Min(1.0f)] private float _damage = 10.0f;
    [SerializeField, Min(0.0f)] private float _spread = 10.0f;

    private WeaponController weaponC;

    private void Awake() => weaponC = GetComponent<WeaponController>();

    private void OnEnable() => weaponC.OnShoot += SpawnBullet;

    private void OnDisable() => weaponC.OnShoot -= SpawnBullet;

    public void SpawnBullet()
    {
        var angle = _shootPoint.eulerAngles;
        angle.x += Random.Range(-_spread, _spread);
        angle.y += Random.Range(-_spread, _spread);

        var bullet = BulletPool.Pool.Get().GetComponent<Bullet>();
        bullet.transform.SetParent(null);
        bullet.transform.position = _shootPoint.position;
        bullet.transform.rotation = Quaternion.Euler(angle);
        bullet.SetValues(_speed, _damage);
        bullet.ResetBullet();
    }
}
