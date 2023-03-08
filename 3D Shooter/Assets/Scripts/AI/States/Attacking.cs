namespace AI.States
{
    public class Attacking : IState
    {
        private readonly EnemyMovement enemyMovement;
        private bool isAttacking;

        public Attacking(EnemyMovement enemyMovement)
        {
            this.enemyMovement = enemyMovement;
        }

        public void FixedUpdate()
        {
            enemyMovement.Agent.SetDestination(enemyMovement.transform.position);
            enemyMovement.transform.LookAt(enemyMovement.Hero);

            if (isAttacking)
                return;

            enemyMovement.Animator.SetTrigger("Attack");
            isAttacking = true;
        }
    }
}