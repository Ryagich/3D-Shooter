using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdWeaponDamageArea : MonoBehaviour
{
    public event Action<HpController> Hit;
    [HideInInspector] public bool isAttack = false;

    private Collider area;
    private readonly List<Collider> checkedBoxes = new();

    private void Awake()
    {
        area = GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        if (!isAttack)
            return;
        var colliders = Physics.OverlapBox(area.bounds.center,
                                           area.bounds.extents / 2);
        foreach (var collider in colliders)
        {
            if (checkedBoxes.Contains(collider))
                return;
            checkedBoxes.Add(collider);
            var hp = collider.gameObject.GetComponent<HpController>();
            if (hp)
                Hit?.Invoke(hp);
        }
    }

    public void EndAttack()
    {
        isAttack = false;
        checkedBoxes.Clear();
    }
}
