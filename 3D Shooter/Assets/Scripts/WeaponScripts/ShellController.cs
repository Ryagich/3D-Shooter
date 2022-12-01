using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellController : MonoBehaviour
{
    [SerializeField] private GameObject _shell;
    [SerializeField] private Transform _shellPoint;
    [SerializeField, Min(0.0f)] private float _shellImpulse = 5.0f;

    private WeaponController weaponC;

    private void Awake()
    {
        weaponC = GetComponent<WeaponController>();
    }

    private void OnEnable()
    {
        weaponC.OnShoot += InstantiateShell;
    }

    private void OnDisable()
    {
        weaponC.OnShoot -= InstantiateShell;
    }

    public void InstantiateShell()
    {
        var shell = Instantiate(_shell, _shellPoint.position, _shellPoint.rotation);
        var impulse = new Vector3(UnityEngine.Random.Range(-_shellImpulse, _shellImpulse), 0, 0);
        shell.GetComponent<Rigidbody>().AddForce(impulse, ForceMode.Impulse);
        Destroy(shell, 2.0f);
    }
}
