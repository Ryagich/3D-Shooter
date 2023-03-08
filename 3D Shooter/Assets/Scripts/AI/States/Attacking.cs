using UnityEngine.Events;

namespace AI.States
{
    public class Attacking : State
    {
        public UnityEvent OnAttack { get; } = new();
        private readonly ZombieLogic zombieLogic;

        public Attacking(ZombieLogic zombieLogic)
        {
            this.zombieLogic = zombieLogic;
        }

        public override void FixedUpdate()
        {
            zombieLogic.Agent.SetDestination(zombieLogic.transform.position);
            zombieLogic.transform.LookAt(zombieLogic.Hero);

            if (zombieLogic.IsAttacking)
                return;

            OnAttack.Invoke();
            zombieLogic.IsAttacking = true;
        }
        
    }
}