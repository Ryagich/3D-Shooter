using UnityEngine;
using Utils;

namespace UI
{
    public class NextDayPopup : MonoBehaviour
    {
        public PopupController CannotNextDay;

        public void Show()
        {
            if (DaysManager.Instance.IsDayCompleted())
            {
                DaysManager.Instance.StartNextDay();
            }
            else
            {
                CannotNextDay.Show();
            }
        }
    }
}