namespace AI.States
{
    public class HeroChasing : IState
    {
        private readonly EnemyMovement enemyMovement;

        public HeroChasing(EnemyMovement enemyMovement)
        {
            this.enemyMovement = enemyMovement;
        }

        public void FixedUpdate()
        {
            enemyMovement.Agent.SetDestination(enemyMovement.Hero.position);
        }
    }
}