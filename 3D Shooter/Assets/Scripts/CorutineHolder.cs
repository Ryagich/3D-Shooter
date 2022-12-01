using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorutineHolder : MonoBehaviour
{
    public static CorutineHolder Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
