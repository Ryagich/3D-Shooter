using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateIndicator : MonoBehaviour
{
    [SerializeField] private Renderer _r;

    public void Angry()
    {
        _r.material.color = Color.red;
    }

    public void Patroling()
    {
        _r.material.color = Color.white;
    }
}
