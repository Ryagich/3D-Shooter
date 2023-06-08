using System;
using System.Linq;
using UnityEngine;

namespace Utils
{
    public class DayController : MonoBehaviour
    {
        public int DayIndex;
        public GameObject Day;

        public bool Objective = true;

        public void SetObjectiveComplete() => Objective = true;
        
        public void StartDay()
        {
            Day.SetActive(true);
        }

        public void EndDay()
        {
            Day.SetActive(false);
        }

        public bool IsCompleted()
        {
            return FindObjectsOfType<ZombieLogic>()
                .Select(x => x.GetComponent<HpController>())
                .All(x => !x.IsAlive) && Objective;
        }
    }
}