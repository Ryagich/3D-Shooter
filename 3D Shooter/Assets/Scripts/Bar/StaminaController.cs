using JetBrains.Annotations;
using UnityEngine;

namespace Bar
{
    public class StaminaController : MonoBehaviour
    {
        [field: SerializeField] [field: Min(0)] public float MaxStamina { get; private set; } = 100;
        [field: SerializeField] public float RegenerationSpeed { get; private set; } = 1;

        [field: SerializeField] [CanBeNull] public Fader Fader { get; private set; }
        [field: SerializeField] [CanBeNull] public BarView BarView { get; private set; }

        public float Stamina => barModel.Amount;

        private BarModel barModel;

        private void Awake()
        {
            barModel = new BarModel(MaxStamina, MaxStamina);

            if (BarView)
                barModel.AmountChanged += BarView.UpdateBar;

            if (Fader)
                barModel.AmountChanged += (_, _) => Fader.Show(true);
        }

        public void FixedUpdate()
        {
            ChangeAmount(RegenerationSpeed * Time.fixedDeltaTime);
        }

        public void ChangeAmount(float value)
        {
            var nextStaminaValue = Mathf.Clamp(barModel.Amount + value, 0, barModel.MaxAmount);
            var delta = nextStaminaValue - barModel.Amount;

            if (delta == 0)
                return;

            barModel.ChangeAmount(value);

            if (BarView)
                BarView.ChangeState(!barModel.IsMax);
        }
    }
}