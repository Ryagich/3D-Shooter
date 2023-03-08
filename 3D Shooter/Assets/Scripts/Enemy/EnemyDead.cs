using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDead : MonoBehaviour
{
    private Animator animator;
    private HpController hp;
    private ZombieLogic movement;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        hp = GetComponent<HpController>();
        movement = GetComponent<ZombieLogic>();

        hp.Deaded += Die;
    }

    private void Die()
    {
        animator.SetTrigger("isDead");
        gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
        GetComponent<NavMeshAgent>().speed = 0.0f;
        movement.enabled = false;
        Destroy(gameObject, 7.0f);
    }
}