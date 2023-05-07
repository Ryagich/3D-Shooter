using System.Linq;
using AI;
using AI.States;
using JetBrains.Annotations;
using Stealth;
using UnityEngine;
using UnityEngine.AI;
using Utils;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(EntityVisualDetector))]
[RequireComponent(typeof(HpController), typeof(EntitySoundDetector))]
public class ZombieLogic : MonoBehaviour
{
    [field: SerializeField] public float Speed { get; set; }

    [field: Header("Random walking")]
    [field: SerializeField] public float WalkPointRange { get; private set; } = 15;
    [field: SerializeField] public LayerMask GroundMask { get; private set; }
    [field: SerializeField, Min(0)] public float WalkingSpeed { get; private set; } = 0.5f;

    [field: Header("Attacking")]
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public EnemyAttack AttackObject { get; private set; }

    [field: Header("Hero chasing")]
    [field: SerializeField] public float ChasingRange { get; private set; } = 50;
    [field: SerializeField, Min(0)] public float ChasingSpeed { get; private set; } = 1f;

    [Header("Debug")]
    [SerializeField] private bool _logsEnabled;
    [SerializeField] private bool _drawPath;

    public NavMeshAgent Agent { get; private set; }
    public Animator Animator { get; private set; }
    public Transform Hero { get; private set; }
    public bool IsAttacking { get; set; }

    private StateMachine stateMachine;
    private ZombieAnimator animator;

    private void Awake()
    {
        Hero = GameObject.Find("Hero").transform;
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        stateMachine = new StateMachine();

        var detector = GetComponent<EntityVisualDetector>();
        var soundDetector = GetComponent<EntitySoundDetector>();

        var randomWalking = new RandomWalking(this);
        var heroChasing = new HeroChasing(this);
        var attacking = new Attacking(this);

        var toHeroChasing = new StateTransition(
            () => soundDetector.CheckDetection() || detector.CheckDetection(), heroChasing);

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

        var hpController = GetComponent<HpController>();
        animator = new ZombieAnimator(this, Animator, attacking, hpController);
    }

    private void FixedUpdate()
    {
        stateMachine.UpdateStates();
        stateMachine.CurrentState.FixedUpdate();

        if (_logsEnabled)
            Debug.Log($"State of {name}: {stateMachine.CurrentState.GetType().Name}");
    }

    private void Update()
    {
        animator.Update();
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

    private void OnDrawGizmosSelected()
    {
        if (!_drawPath || Agent == null)
            return;

        Gizmos.color = Color.red;

        var previousCorner = transform.position;
        foreach (var corner in Agent.path.corners.Append(Agent.destination))
        {
            Gizmos.DrawLine(previousCorner, corner);
            previousCorner = corner;
        }

        Gizmos.DrawSphere(Agent.destination, 0.1f);
    }
}