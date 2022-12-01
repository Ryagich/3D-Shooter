using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDeath : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Hp>().OnDead += Destroy; 
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
