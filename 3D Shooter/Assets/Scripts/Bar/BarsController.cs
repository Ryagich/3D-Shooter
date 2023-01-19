using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarsController : MonoBehaviour
{
    [SerializeField, Min(0.0f)] private float _maxHunger = 100.0f, _hunger = 50.0f;
    [SerializeField, Min(0.0f)] private float _maxThirst = 100.0f, _thirst = 80.0f;
    [SerializeField, Min(0.0f)] private float _maxInfection = 100.0f, _infection = 0.0f;
}
