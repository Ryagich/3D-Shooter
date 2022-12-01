using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static Pool Pool { get; private set; }
    [SerializeField] private Bullet _bulletPref;
    [SerializeField] private Transform _parent;

    private void Awake()
    {
        Pool = new Pool(_bulletPref.gameObject, 60, _parent);
    }
}
