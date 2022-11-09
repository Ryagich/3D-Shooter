using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Bullet _bullet;
    [SerializeField] private GameObject _shell;
    [SerializeField] private Transform _shootPoint, _shellPoint;
    [SerializeField, Min(0.0f)] private float _cooldownTime = 0.5f, _shellImpulse = 5.0f;

    private bool isReady = true;
    private bool isMouse = false;

    [Header("Bullet Stats")]
    [SerializeField, Min(1.0f)] private float _speed = 5.0f;
    [SerializeField, Min(1.0f)] private float _damage = 10.0f;

    private void Awake()
    {
        InputHandler.OnMouseDown += StartShooting;
        InputHandler.OnMouseUp += StopShooting;
    }

    private void StartShooting()
    {
        isMouse = true;
        Shoot();
    }

    private void StopShooting()
    {
        isMouse = false;
    }

    public void Shoot()
    {
        if (isMouse && isReady)
        {
            var bullet = Instantiate(_bullet, _shootPoint.position, _shootPoint.rotation);
            bullet.SetValues(_speed, _damage);
            InstantiateShell();
            isReady = false;
            StartCoroutine(Cooldown());
        }
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(_cooldownTime);
        isReady = true;
        Shoot();
    }

    private void InstantiateShell()
    {
        var shell = Instantiate(_shell, _shellPoint.position, _shellPoint.rotation);
        var impulse = new Vector3(Random.Range(-_shellImpulse, _shellImpulse), 0, 0);
        shell.GetComponent<Rigidbody>().AddForce(impulse, ForceMode.Impulse);
        Destroy(shell, 2.0f);
    }
}
