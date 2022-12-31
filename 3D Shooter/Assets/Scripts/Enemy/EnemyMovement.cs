using UnityEngine;
using UnityEngine.AI;

[ExecuteAlways]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private LayerMask _groundMask, _heroMask;
    [SerializeField] private float _sightRange, _attackRange;
    [SerializeField] private bool _canPatroling = false;
    [SerializeField] private Vector3 _walkPoint;
    [SerializeField] private float _walkPointRange;
    [SerializeField] private EnemyAttack _attack;

    private NavMeshAgent agent;
    private Transform hero;
    private bool isAttacling = false, walkPointSet = false;
    private bool heroInSightRange, heroInAttackRange;
    private Animator animator;

    private void Awake()
    {
        hero = GameObject.Find("Hero").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        heroInSightRange = Physics.CheckSphere(transform.position,
                                               _sightRange, _heroMask);
        heroInAttackRange = Physics.CheckSphere(transform.position,
                                               _attackRange, _heroMask);

        if (!heroInSightRange && !heroInAttackRange && _canPatroling)
            Patroling();
        else if (heroInSightRange && !heroInAttackRange)
            ChaseHero();
        else if (heroInAttackRange)
            AttackHero();
    }

    private void Patroling()
    {
        if (!gameObject)
            return;
        if (!walkPointSet)
            SearchWalkPoint();

        agent.SetDestination(_walkPoint);
        var distance = transform.position - _walkPoint;

        transform.LookAt(_walkPoint);

        if (distance.magnitude < 1.0f)
            walkPointSet = false;
        animator.SetBool("isWalk", true);
        animator.SetBool("IsIdle", false);
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

        if (!isAttacling)
        {
            animator.SetTrigger("Attack");
            isAttacling = true;
        }
    }

    public void Attack()
    {
        _attack.Attack();
    }

    public void SetAttackState()
    {
        isAttacling = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _sightRange);
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}
