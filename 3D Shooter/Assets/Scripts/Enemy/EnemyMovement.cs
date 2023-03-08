using AI;
using AI.States;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

[ExecuteAlways]
[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class EnemyMovement : MonoBehaviour
{
    [field: SerializeField] public Vector3 WalkPoint { get; set; }
    [field: SerializeField] public float WalkPointRange { get; private set; } = 15;
    [field: SerializeField] public float SightRange { get; private set; } = 100;
    [field: SerializeField] public LayerMask GroundMask { get; private set; }

    [SerializeField] private LayerMask _heroMask;
    [SerializeField] private float _sightRange, _attackRange;
    [SerializeField] private EnemyAttack _attack;

    public NavMeshAgent Agent { get; private set; }
    public Animator Animator { get; private set; }
    public Transform Hero { get; private set; }
    public bool IsAttacking { get; private set; }

    private bool heroInSightRange, heroInAttackRange;

    private IState randomWalking;
    private IState attacking;
    private IState heroChasing;

    private void Awake()
    {
        Hero = GameObject.Find("Hero").transform;
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();

        randomWalking = new RandomWalking(this);
        attacking = new Attacking(this);
        heroChasing = new HeroChasing(this);
    }

    private void FixedUpdate()
    {
        heroInSightRange = Physics.CheckSphere(transform.position,
            _sightRange, _heroMask);

        heroInAttackRange = Physics.CheckSphere(transform.position,
            _attackRange, _heroMask);

        if (heroInAttackRange)
            attacking.FixedUpdate();
        else if (heroInSightRange)
            heroChasing.FixedUpdate();
        else
            randomWalking.FixedUpdate();
    }

    [UsedImplicitly]
    public void Attack()
    {
        _attack.Attack();
    }

    [UsedImplicitly]
    public void SetAttackState()
    {
        IsAttacking = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _sightRange);
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}