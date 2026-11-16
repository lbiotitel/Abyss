using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatBase 
{

    protected float minValue;
    protected float maxValue;

    protected float baseValue;
    protected EStatType statType;

    public event Action<EStatType,float,float> OnStatValueChanged;

    public StatBase(float value, EStatType statType)
    {
        this.baseValue = value;
        this.statType = statType;
    }

    public abstract float GetValue();

    public abstract void AddModifier(StatModifier statModifier);

    public abstract void RemoveModifier(StatModifier statModifier);

    public void TryInvokeOnValueChanged(float prevValue, float newValue)
        => OnStatValueChanged?.Invoke(statType,prevValue, newValue);
    
}
