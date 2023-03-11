using UnityEngine;

namespace Stealth
{
    public class EntitySoundDetector : MonoBehaviour
    {
        [field: SerializeField, Min(0)] public float HearingRadius { get; private set; }
        [field: SerializeField] public LayerMask Detects { get; private set; }

        public bool CheckDetection()
        {
            return Physics.CheckSphere(transform.position, HearingRadius, Detects);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, HearingRadius);
        }
    }
}