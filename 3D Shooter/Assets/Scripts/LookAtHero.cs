using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtHero : MonoBehaviour
{
    private Transform hero;

    private void Awake()
    {
        hero = GameObject.FindGameObjectWithTag("Hero").transform;        
    }

    private void FixedUpdate()
    {
        transform.LookAt(hero);
    }
}
