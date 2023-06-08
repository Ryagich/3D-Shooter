using System;
using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    public class Trigger : MonoBehaviour
    {
        public UnityEvent HeroEnter = new();

        public bool OneTime;

        private bool executed;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Hero") || (OneTime && executed))
                return;

            Debug.Log("Enter");
            executed = true;
            HeroEnter.Invoke();
        }
    }
}