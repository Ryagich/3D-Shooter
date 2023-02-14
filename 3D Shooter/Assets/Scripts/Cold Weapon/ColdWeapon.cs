using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ColdWeapon : MonoBehaviour
{
    [SerializeField, Min(.0f)] private float _damage = 1.0f;
    [SerializeField] private ColdWeaponDamageArea _damageArea;

    private Animator animC;

    private void Awake()
    {
        animC = GetComponent<Animator>();
        _damageArea.Hit += SetDamage;
    }

    public void StartAttack() => _damageArea.isAttack = true;
    public void EndAttack() => _damageArea.EndAttack();
    private void Attack() => animC.SetTrigger("Attack");
    private void SetDamage(HpController hp) => hp.ChangeAmount(-_damage);
    private void OnEnable() => InputHandler.LeftMouseDowned += Attack;
    private void OnDisable() => InputHandler.LeftMouseDowned -= Attack;
    private void OnDestroy() => InputHandler.LeftMouseDowned -= Attack;
}
