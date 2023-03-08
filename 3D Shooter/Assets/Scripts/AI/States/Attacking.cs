namespace AI.States
{
    public class Attacking : State
    {
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

            zombieLogic.Animator.SetTrigger("Attack");
            zombieLogic.IsAttacking = true;
        }
    }
}