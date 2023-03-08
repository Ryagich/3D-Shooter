namespace AI.States
{
    public class HeroChasing : State
    {
        private readonly ZombieLogic zombieLogic;

        public HeroChasing(ZombieLogic zombieLogic)
        {
            this.zombieLogic = zombieLogic;
        }

        public override void FixedUpdate()
        {
            zombieLogic.Agent.SetDestination(zombieLogic.Hero.position);
        }
    }
}