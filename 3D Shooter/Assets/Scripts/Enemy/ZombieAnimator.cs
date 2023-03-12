using AI.States;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAnimator
{
    private readonly ZombieLogic zombieLogic;
    private readonly Animator animator;

    public ZombieAnimator(ZombieLogic zombieLogic, Animator animator, Attacking attackingState,
        HpController hpController)
    {
        this.zombieLogic = zombieLogic;
        this.animator = animator;

        attackingState.OnAttack.AddListener(() => animator.SetTrigger("OnAttack"));
        hpController.OnDeath.AddListener(() => animator.SetTrigger("OnDeath"));
    }

    public void Update()
    {
        animator.SetFloat("Speed", zombieLogic.Speed);
    }
}