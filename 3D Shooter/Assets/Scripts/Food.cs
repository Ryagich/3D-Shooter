using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private float _hp, _hunger, _thirst, _immunity;

    private ItemModel model;
    private BarsController barsC;
    private HpController hpC;

    private void Awake()
    {
        var hero = GameObject.FindGameObjectWithTag("Hero");
        barsC = hero.GetComponent<BarsController>();
        hpC = hero.GetComponent<HpController>();
    }

    private void Eat()
    {
        model ??= GetComponent<HandItem>().ItemM;
        model.Amount--;

        hpC.ChangeAmount(_hp);
        barsC.ChangeStats(_hunger, _thirst, _immunity);
    }

    private void OnEnable() => InputHandler.LeftMouseDowned += Eat;

    private void OnDisable() => InputHandler.LeftMouseDowned -= Eat;

    private void OnDestroy() => InputHandler.LeftMouseDowned -= Eat;
}
