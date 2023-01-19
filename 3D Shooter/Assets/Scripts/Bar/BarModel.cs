using UnityEngine;
using System;

public class BarModel
{
    public event Action<float, float> OnAmountChanged;

    private float amount, maxAmount;

    public float Amount => amount;
    public float MaxAmount => maxAmount;
    public bool IsMax => amount == maxAmount;

    public BarModel(float amount, float maxAmount)
    {
        this.amount = amount;
        this.maxAmount = maxAmount;
    }

    public void ChangeAmount(float value)
    {
        amount = Mathf.Clamp(amount + value, 0, maxAmount);
        OnAmountChanged?.Invoke(amount, maxAmount);
    }
}
