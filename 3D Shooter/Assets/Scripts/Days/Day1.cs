using UI;
using UnityEngine;
using Utils;

namespace Days
{
    public class Day1 : MonoBehaviour
    {
        public Transform Target;

        private void Start()
        {
            WaypointManager.Instance.SetTarget(Target);
        }
    }
}