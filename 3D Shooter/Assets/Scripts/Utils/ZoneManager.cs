using System.Threading;

namespace Utils
{
    public class ZoneManager : MonoBehaviourSingleton<ZoneManager>
    {
        public float Damage = 0.05f;
        public HpController Hero;
        public int SafeZones;

        public void Reload()
        {
            SafeZones = 0;
        }

        private void FixedUpdate()
        {
            if (SafeZones == 0)
                Hero.ChangeAmount(-Damage);
        }

        public void IncrementSafeZone()
        {
            SafeZones++;
        }

        public void DecrementSafeZone()
        {
            SafeZones--;
        }
    }
}