using System.Linq;
using UI;
using UnityEngine;
using Utils;

namespace Days
{
    public class Day0 : MonoBehaviour
    {
        public WaypointManager WaypointManager;
        public Transform SleepTarget;
        public DayController Day0_;

        private bool isCompleted;

        public void Update()
        {
            if (isCompleted)
                return;

            var allDead = FindObjectsOfType<ZombieLogic>()
                .Select(x => x.GetComponent<HpController>())
                .All(x => !x.IsAlive);

            if (allDead)
            {
                isCompleted = true;
                WaypointManager.SetTarget(SleepTarget);
                Day0_.SetObjectiveComplete();
            }
        }
    }
}