using UnityEngine;

namespace UI
{
    public class WaypointManager : MonoBehaviourSingleton<WaypointManager>
    {
        public Waypoint Waypoint;

        public void SetTarget(Transform target)
        {
         //   Waypoint.gameObject.SetActive(true);
            Waypoint.target = target;
        }

        public void ClearTarget()
        {
            Waypoint.target = null;
            // Waypoint.gameObject.SetActive(false);
        }
    }
}