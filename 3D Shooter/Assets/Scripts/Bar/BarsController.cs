using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarsController : MonoBehaviour
{
    [SerializeField, Min(0.0f)] private float _maxHunger = 100.0f, _hunger = 50.0f;
    [SerializeField, Min(0.0f)] private float _maxThirst = 100.0f, _thirst = 80.0f;
    [SerializeField, Min(0.0f)] private float _maxInfection = 100.0f, _infection = 100.0f;

    private BarModel hungerM, thirstM, infectionM;

    private void Awake()
    {
        hungerM = new BarModel(_hunger, _maxHunger);
        thirstM = new BarModel(_thirst, _maxThirst);
        infectionM = new BarModel(_infection, _maxInfection);
     }
}
