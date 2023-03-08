using UnityEngine;

namespace AI.States
{
    public class RandomWalking : IState
    {
        private readonly EnemyMovement enemyMovement;
        private bool isWalkPointSet;

        public RandomWalking(EnemyMovement enemyMovement)
        {
            this.enemyMovement = enemyMovement;
        }

        public void FixedUpdate()
        {
            if (!enemyMovement.gameObject)
                return;

            if (isWalkPointSet)
                SearchWalkPoint();

            enemyMovement.Agent.SetDestination(enemyMovement.WalkPoint);
            enemyMovement.transform.LookAt(enemyMovement.WalkPoint);

            var distance = enemyMovement.transform.position - enemyMovement.WalkPoint;
            if (distance.magnitude < 1.0f)
                isWalkPointSet = false;

            enemyMovement.Animator.SetBool("isWalk", true);
            enemyMovement.Animator.SetBool("IsIdle", false);
        }

        private void SearchWalkPoint()
        {
            var rZ = Random.Range(enemyMovement.WalkPointRange, enemyMovement.WalkPointRange);
            var rX = Random.Range(enemyMovement.WalkPointRange, enemyMovement.WalkPointRange);

            var position = enemyMovement.transform.position;
            var point = new Vector3(position.x + rX, position.y, position.z + rZ);
            enemyMovement.WalkPoint = point;

            if (Physics.Raycast(point, -enemyMovement.transform.up, 2.0f, enemyMovement.GroundMask))
                isWalkPointSet = true;
        }
    }
}