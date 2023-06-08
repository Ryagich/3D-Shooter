using UI;

namespace Utils
{
    public class DaysManager : MonoBehaviourSingleton<DaysManager>
    {
        public int CurrentDay;
        public DayController[] Days;

        public Stat Hunger;
        public Stat Water;
        public HpController Hp;

        public void StartNextDay()
        {
            Hunger.ChangeAmount(-10);
            Water.ChangeAmount(-10);
            Hp.ChangeAmount(Hp._maxHp - Hp.BarM.Amount);

            Days[CurrentDay].EndDay();
            CurrentDay++;
            Days[CurrentDay].StartDay();
            WaypointManager.Instance.ClearTarget();
        }

        public bool IsDayCompleted()
        {
            return Days[CurrentDay].IsCompleted();
        }
    }
}