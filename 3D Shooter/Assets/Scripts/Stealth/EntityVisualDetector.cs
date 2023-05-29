using System.Linq;
using UnityEngine;
using Utils;

namespace Stealth
{
    [ExecuteAlways]
    public class EntityVisualDetector : MonoBehaviour
    {
        [field: SerializeField] public LayerMask Detects { get; private set; }
        [field: SerializeField] [field: Min(0.1f)] public float Radius { get; private set; } = 10;
        [field: SerializeField] [field: Range(0, 360)] public float HorizontalFov { get; private set; } = 90;
        [field: SerializeField] public Vector3 DetectionPointOffset { get; private set; }

        [field: Header("Obstacles")]
        [field: SerializeField] public LayerMask Obstacles { get; private set; }
        [field: SerializeField]
        [field: Range(0.01f, 100)] public float ObstacleDetectionPrecision { get; private set; } = 5;

        [Header("Debug")]
        [SerializeField] private bool _logsEnabled;
        [SerializeField] [field: Range(0, 2)] private int _drawDebug;

        private Vector3 DetectionPoint => transform.localRotation * DetectionPointOffset + transform.position;

        // 321
        // 4*0
        // 567
        private static readonly (int X, int Y)[] IterationPattern =
        {
            (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0), (-1, -1), (0, -1), (1, -1)
        };

        public bool CheckDetection()
        {
            var colliders = Physics.OverlapSphere(DetectionPoint, Radius, Detects);
            //Log($"Entities in radius: {colliders.Length}");

            var detected = colliders
                .Where(IsInFov)
                .Where(HasNoObstaclesBetween)
                .FirstOrDefault();

            //Log($"Detected: {detected?.gameObject.name}");
            return detected != null;
        }

        private bool IsInFov(Collider target)
        {
            var forward = transform.forward;
            var toTarget = target.transform.position - DetectionPoint;

            var angle = Vector2.Angle(new Vector2(forward.x, forward.z), new Vector2(toTarget.x, toTarget.z));

            var isInFov = angle <= HorizontalFov / 2;

            //Log($"Entity: {target.gameObject.name}, Angle: {angle}, IsInFov: {isInFov}");
            return isInFov;
        }

        private bool HasNoObstaclesBetween(Collider target)
        {
            var boundsSize = target.bounds.size;
            var boundingSphereDiameter = Mathf.Sqrt(boundsSize.x * boundsSize.x
                                                    + boundsSize.y * boundsSize.y
                                                    + boundsSize.z * boundsSize.z);

            var distanceToTarget = Vector3.Distance(target.bounds.center, DetectionPoint);
            var maxAngle = Mathf.Atan2(boundingSphereDiameter / 2, distanceToTarget) * Mathf.Rad2Deg;

            var directionToTarget = (target.transform.position - DetectionPoint).normalized;

            for (var i = 0f; i < maxAngle; i += maxAngle / ObstacleDetectionPrecision)
                foreach (var (x, y) in IterationPattern)
                {
                    var rotation = Quaternion.AngleAxis(x * i, Vector3.left)
                                   * Quaternion.AngleAxis(y * i, Vector3.up);

                    var direction = rotation * directionToTarget;

                    if (_drawDebug == 2)
                        Debug.DrawRay(DetectionPoint, direction * Radius);

                    var hits = Physics.RaycastAll(DetectionPoint, direction, Radius, Obstacles | Detects);

                    if (hits.Length == 0)
                        continue;

                    var hit = hits.MinBy(h => h.distance);
                    if (Detects.Contains(hit.transform.gameObject.layer))
                    {
                        if (_drawDebug == 1)
                            Debug.DrawLine(DetectionPoint, hit.point, Color.magenta);

                        return true;
                    }
                }

            return false;
        }

#if UNITY_EDITOR
        private void Log(string message)
        {
            if (_logsEnabled)
                Debug.Log(message);
        }

        private void Update()
        {
            CheckDetection();
        }
#endif
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(DetectionPoint, Radius);

            Gizmos.DrawSphere(DetectionPoint, 0.1f);

            Gizmos.color = Color.red;
            var front = transform.forward * Radius;
            var rightFovEdge = Quaternion.AngleAxis(HorizontalFov / 2, transform.up) * front + DetectionPoint;
            Gizmos.DrawLine(DetectionPoint, rightFovEdge);
            var leftFovEdge = Quaternion.AngleAxis(-HorizontalFov / 2, transform.up) * front + DetectionPoint;
            Gizmos.DrawLine(DetectionPoint, leftFovEdge);
        }
    }
}