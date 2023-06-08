using System;
using System.Linq;
using UnityEngine;

namespace Utils
{
    public class SafeZone : MonoBehaviour
    {
        private HpController hero;
        private bool isIn;

        private void Awake()
        {
            hero = FindObjectsOfType<HpController>()
                .First(x => x.CompareTag("Hero"));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == hero.gameObject)
            {
                ZoneManager.Instance.IncrementSafeZone();
                isIn = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == hero.gameObject)
            {
                isIn = false;
                ZoneManager.Instance.DecrementSafeZone();
            }
        }

        private void OnDisable()
        {
            if (isIn)
                ZoneManager.Instance.DecrementSafeZone();
        }
    }
}