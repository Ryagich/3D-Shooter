using System;
using System.Linq;
using UI;
using UnityEngine;

namespace Days
{
    public class Day5: MonoBehaviour
    {
        public PopupController popup;
        
        private void Update()
        {
            var allDead = FindObjectsOfType<ZombieLogic>()
                .Select(x => x.GetComponent<HpController>())
                .All(x => !x.IsAlive);

            if (allDead)
            {
                popup.Show();
            }
        }
    }
}