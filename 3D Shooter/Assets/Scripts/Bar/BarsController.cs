using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BarsController : MonoBehaviour
{
    public Stat Hunger => _hunger;
    public Stat Thirst => _thirst;
    public Stat Immunity => _immunity;

    [SerializeField] private Stat _hunger, _thirst, _immunity;

    public void ChangeStats(float hunger, float thirst, float immunity)
    {
        _hunger.ChangeAmount(hunger);
        _thirst.ChangeAmount(thirst);
        _immunity.ChangeAmount(immunity);
    }
}
