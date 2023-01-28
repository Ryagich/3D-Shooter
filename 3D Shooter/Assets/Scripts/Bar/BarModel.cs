using UnityEngine;
using System;

public class BarModel
{
    public event Action<float, float> AmountChanged;

    public float Amount => amount;
    public float MaxAmount => maxAmount;
    public bool IsMax => amount == maxAmount;

    private float amount, maxAmount;

    public BarModel(float amount, float maxAmount)
    {
        this.amount = amount;
        this.maxAmount = maxAmount;
    }

    public void ChangeAmount(float value)
    {
        if (value == .0f)
            return;

        amount = Mathf.Clamp(amount + value, 0, maxAmount);
        AmountChanged?.Invoke(amount, maxAmount);
    }
}
