using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDeath : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<HpController>().Deaded += Destroy; 
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
