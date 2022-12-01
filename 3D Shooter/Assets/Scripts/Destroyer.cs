using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [SerializeField] private float _liveTime = 10.0f;

    private void Awake()
    {
        Destroy(gameObject, _liveTime);
    }
}
