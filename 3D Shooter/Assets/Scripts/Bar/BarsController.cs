using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarsController : MonoBehaviour
{
    public Stat Hunger => _hunger;
    public Stat Thirst => _thirst;
    public Stat Immunity => _immunity;

    [SerializeField] private Stat _hunger;
    [SerializeField] private Stat _thirst;
    [SerializeField] private Stat _immunity;
}
