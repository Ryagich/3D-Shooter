using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] private GameObject _objPref;
    [SerializeField] private float _coolDown = 2.0f;
    [SerializeField] private bool _isSingle = false;
    private GameObject obj;

    private void Awake()
    {
        if (_isSingle)
            SpawnObj2();
        else
            SpawnObj1();
    }

    private void SpawnObj1()
    {
        Instantiate(_objPref, transform.position, transform.rotation);
        Invoke(nameof(SpawnObj1), _coolDown);
    }

    private void SpawnObj2()
    {
        if (!obj)
        {
            obj = Instantiate(_objPref, transform.position, transform.rotation);

        }
        Invoke(nameof(SpawnObj2), _coolDown);
    }
}
