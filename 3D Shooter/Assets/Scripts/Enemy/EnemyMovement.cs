using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

[ExecuteAlways]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private LayerMask _groundMask, _heroMask;
    [SerializeField] private float _timeBetweenAttacks;
    [SerializeField] private float _sightRange, _attackRange;
    [SerializeField] private bool _canPatroling = false;
    [SerializeField] private Vector3 _walkPoint;
    [SerializeField] private float _walkPointRange;

    private NavMeshAgent agent;
    private Transform hero;
    private bool alreadyAttacked, walkPointSet = false;
    private bool heroInSightRange, heroInAttackRange;
    private EnemyStateIndicator indicator;

    private void Awake()
    {
        hero = GameObject.Find("Hero").transform;
        agent = GetComponent<NavMeshAgent>();
        indicator = GetComponent<EnemyStateIndicator>();
    }

    private void FixedUpdate()
    {
        heroInSightRange = Physics.CheckSphere(transform.position,
                                               _sightRange, _heroMask);
        heroInAttackRange = Physics.CheckSphere(transform.position,
                                               _attackRange, _heroMask);

        if (!heroInSightRange && !heroInAttackRange && _canPatroling)
            Patroling();
        if (heroInSightRange && !heroInAttackRange)
            ChaseHero();
        if (heroInAttackRange)
            AttackHero();
    }

    private void Patroling()
    {
        if (!walkPointSet)
            SearchWalkPoint();

        agent.SetDestination(_walkPoint);
        var distance = transform.position - _walkPoint;

        indicator.Patroling();
        transform.LookAt(_walkPoint);

        if (distance.magnitude < 1.0f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        var rZ = Random.Range(-_walkPointRange, _walkPointRange);
        var rX = Random.Range(-_walkPointRange, _walkPointRange);
        _walkPoint = new Vector3(transform.position.x + rX,
                                 transform.position.y,
                                 transform.position.z + rZ);

        if (Physics.Raycast(_walkPoint, -transform.up, 2.0f, _groundMask))
            walkPointSet = true;
    }

    private void ChaseHero()
    {
        agent.SetDestination(hero.position);
        transform.LookAt(hero);
    }

    private void AttackHero()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(hero);
        indicator.Angry();

        if (!alreadyAttacked)
        {
            //Attack Code here

            //end attackCode

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack),_timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _sightRange);
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}
