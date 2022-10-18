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

    [Header("Bullet Stats")]
    [SerializeField, Min(1.0f)] private float _speed = 5.0f;
    [SerializeField, Min(1.0f)] private float _damage = 10.0f;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && isReady)
            Shoot();
    }

    public void Shoot()
    {
        var bullet = Instantiate(_bullet, _shootPoint.position, _shootPoint.rotation);
        bullet.SetValues(_speed);
        InstantiateShell();
        isReady = false;
        Camera.main.GetComponent<MonoBehaviour>().StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(_cooldownTime);
        isReady = true;
    }

    private void InstantiateShell()
    {
        var shell = Instantiate(_shell, _shellPoint.position, _shellPoint.rotation);
        var impulse = new Vector3(Random.Range(-_shellImpulse, _shellImpulse), 0, 0);
        shell.GetComponent<Rigidbody>().AddForce(impulse, ForceMode.Impulse);
        Destroy(shell, 2.0f);
    }
}
