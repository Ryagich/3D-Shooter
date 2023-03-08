using AI.States;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAnimator
{
    private readonly Animator animator;
    private readonly NavMeshAgent navMeshAgent;

    public ZombieAnimator(Animator animator, NavMeshAgent navMeshAgent, Attacking attackingState,
        HpController hpController)
    {
        this.animator = animator;
        this.navMeshAgent = navMeshAgent;

        attackingState.OnAttack.AddListener(() => animator.SetTrigger("OnAttack"));
        hpController.OnDeath.AddListener(() => animator.SetTrigger("OnDeath"));
    }

    public void Update()
    {
        animator.SetFloat("Speed", navMeshAgent.speed);
    }
}