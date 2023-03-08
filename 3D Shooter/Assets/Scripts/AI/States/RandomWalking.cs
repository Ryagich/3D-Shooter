using UnityEngine;

namespace AI.States
{
    public class RandomWalking : State
    {
        private readonly ZombieLogic zombieLogic;
        private bool isWalkPointSet;

        private float initialSpeed;

        public RandomWalking(ZombieLogic zombieLogic)
        {
            this.zombieLogic = zombieLogic;
        }

        public override void OnEnter()
        {
            isWalkPointSet = false;
            initialSpeed = zombieLogic.Agent.speed;
            zombieLogic.Agent.speed = zombieLogic.WalkingSpeed;
        }

        public override void FixedUpdate()
        {
            if (!zombieLogic.gameObject)
                return;

            if (!isWalkPointSet)
                SearchWalkPoint();

            zombieLogic.Agent.SetDestination(zombieLogic.WalkPoint);

            var distance = zombieLogic.transform.position - zombieLogic.WalkPoint;
            if (distance.magnitude < 1.0f)
                isWalkPointSet = false;
        }

        public override void OnExit()
        {
            zombieLogic.Agent.speed = initialSpeed;
        }

        private void SearchWalkPoint()
        {
            var rZ = Random.Range(-zombieLogic.WalkPointRange, zombieLogic.WalkPointRange);
            var rX = Random.Range(-zombieLogic.WalkPointRange, zombieLogic.WalkPointRange);

            var position = zombieLogic.transform.position;
            var point = new Vector3(position.x + rX, position.y, position.z + rZ);
            zombieLogic.WalkPoint = point;

            if (Physics.Raycast(point, -zombieLogic.transform.up, 2.0f, zombieLogic.GroundMask))
                isWalkPointSet = true;
        }
    }
}