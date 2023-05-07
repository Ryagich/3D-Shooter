using UnityEngine;
using UnityEngine.AI;

namespace AI.States
{
    public class RandomWalking : State
    {
        private readonly ZombieLogic zombieLogic;

        private float initialSpeed;

        public RandomWalking(ZombieLogic zombieLogic)
        {
            this.zombieLogic = zombieLogic;
        }

        public override void OnEnter()
        {
            initialSpeed = zombieLogic.Agent.speed;
            zombieLogic.Agent.ResetPath();
        }

        public override void FixedUpdate()
        {
            zombieLogic.Speed = zombieLogic.WalkingSpeed;

            var distance = zombieLogic.transform.position - zombieLogic.Agent.destination;

            if (distance.magnitude < 1.0f || !zombieLogic.Agent.hasPath)
            {
                var destinationPoint = FindDestinationPoint();
                if (destinationPoint.HasValue)
                {
                    zombieLogic.Agent.SetDestination(destinationPoint.Value);
                }
            }
        }

        public override void OnExit()
        {
            zombieLogic.Speed = initialSpeed;
        }

        private Vector3? FindDestinationPoint()
        {
            var rZ = Random.Range(-zombieLogic.WalkPointRange, zombieLogic.WalkPointRange);
            var rX = Random.Range(-zombieLogic.WalkPointRange, zombieLogic.WalkPointRange);

            var position = zombieLogic.transform.position;
            var point = new Vector3(position.x + rX, position.y, position.z + rZ);

            var isFoundPoint = NavMesh.SamplePosition(point, out var navMeshHit, zombieLogic.WalkPointRange, 1);

            if (!isFoundPoint)
                return null;

            var directionToNavMesh = navMeshHit.position - point;
            var delta = directionToNavMesh.normalized * zombieLogic.Agent.radius;

            return navMeshHit.position + new Vector3(delta.x, 0, delta.z);
        }
    }
}