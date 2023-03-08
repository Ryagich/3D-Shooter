namespace AI.States
{
    public class HeroChasing : State
    {
        private readonly ZombieLogic zombieLogic;

        private float initialSpeed;

        public HeroChasing(ZombieLogic zombieLogic)
        {
            this.zombieLogic = zombieLogic;
        }

        public override void OnEnter()
        {
            initialSpeed = zombieLogic.Agent.speed;
            zombieLogic.Agent.speed = zombieLogic.ChasingSpeed;
        }

        public override void FixedUpdate()
        {
            zombieLogic.Agent.SetDestination(zombieLogic.Hero.position);
        }

        public override void OnExit()
        {
            zombieLogic.Agent.speed = initialSpeed;
        }
    }
}