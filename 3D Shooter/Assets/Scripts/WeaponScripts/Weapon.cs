using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Bullet _bullet;
    [SerializeField] private GameObject _shell;
    [SerializeField] private Transform _shootPoint, _shellPoint;

    [Header("Bullet Stats")]
    [SerializeField, Min(1.0f)] private float _speed = 5.0f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            Shoot();
    }

    public void Shoot()
    {
        var bullet = Instantiate(_bullet, _shootPoint);
        bullet.SetValues(_speed);
        AddShell();
    }

    private void AddShell()
    {
       var shell = Instantiate(_shell, _shellPoint.position, _shellPoint.rotation);
        Destroy(shell, 2.0f);
    }
}
