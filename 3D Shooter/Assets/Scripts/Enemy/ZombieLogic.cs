using AI;
using AI.States;
using JetBrains.Annotations;
using Stealth;
using UnityEngine;
using UnityEngine.AI;
using Utils;

[ExecuteAlways]
[RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(EntityVisualDetector))]
public class ZombieLogic : MonoBehaviour
{
    [field: Header("Random walking")]
    [field: SerializeField] public Vector3 WalkPoint { get; set; }
    [field: SerializeField] public float WalkPointRange { get; private set; } = 15;
    [field: SerializeField] public LayerMask GroundMask { get; private set; }

    [field: Header("Attacking")]
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public EnemyAttack AttackObject { get; private set; }

    [field: Header("Hero chasing")]
    [field: SerializeField] public float ChasingRange { get; private set; } = 50;

    [Header("Debug")]
    [SerializeField] private bool _logsEnabled;

    public NavMeshAgent Agent { get; private set; }
    public Animator Animator { get; private set; }
    public Transform Hero { get; private set; }
    public bool IsAttacking { get; set; }

    private StateMachine stateMachine;

    private void Awake()
    {
        Hero = GameObject.Find("Hero").transform;
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        stateMachine = new StateMachine();

        var detector = GetComponent<EntityVisualDetector>();

        var randomWalking = new RandomWalking(this);
        var heroChasing = new HeroChasing(this);
        var attacking = new Attacking(this);

        var toHeroChasing = new StateTransition(detector.CheckDetection, heroChasing);
        randomWalking.Transitions.Add(toHeroChasing);

        var toRandomWalking = new StateTransition(
            () => Vector3.Distance(Hero.position, transform.position) > ChasingRange, randomWalking);

        heroChasing.Transitions.Add(toRandomWalking);

        var toAttack = new StateTransition(
            () => Vector3.Distance(Hero.position, transform.position) < AttackRange, attacking);

        var toExit = new StateTransition(
            () => Vector3.Distance(Hero.position, transform.position) > AttackRange, stateMachine.Exit);

        attacking.Transitions.Add(toExit);

        stateMachine.Entry.Transitions.Add(() => true, randomWalking);
        stateMachine.AnyState.Transitions.Add(toAttack);
    }

    private void FixedUpdate()
    {
        stateMachine.UpdateStates();
        stateMachine.CurrentState.FixedUpdate();


        if (_logsEnabled)
            Debug.Log($"State of {name}: {stateMachine.CurrentState.GetType().Name}");
    }

    [UsedImplicitly]
    public void Attack()
    {
        AttackObject.Attack();
    }

    [UsedImplicitly]
    public void SetAttackState()
    {
        IsAttacking = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, AttackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, ChasingRange);
    }
}