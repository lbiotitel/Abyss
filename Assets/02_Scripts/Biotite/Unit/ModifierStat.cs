using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierStat : StatBase
{

    private List<StatModifier> statModifiers;
    private bool isDirty = true;
    private float cachedValue = 0;

    public override float GetValue()
    {
        if (isDirty) cachedValue = CalcModifier();
        return cachedValue;
    }


    public ModifierStat(float value, EStatType statType) : base(value, statType)
    {
        statModifiers = new List<StatModifier>();
    }

    public override void AddModifier(StatModifier statModifier)
    {
        float prevValue = GetValue();
        statModifiers.Add(statModifier);
        isDirty = true;
        TryInvokeOnValueChanged(prevValue, GetValue());
    }

    public override void RemoveModifier(StatModifier statModifier)
    {
        float prevValue = GetValue();
        statModifiers.RemoveAll((s) => s.Resource == statModifier.Resource);
        isDirty = true;
        TryInvokeOnValueChanged(prevValue, GetValue());
    }


    private float CalcModifier()
    {
        float percent = 0;
        float value = baseValue;

        foreach (var modifier in statModifiers)
        {
            if (modifier.StatModeType == EStatModeType.ADD)
            {
                value += modifier.Value;
            }
            else if (modifier.StatModeType == EStatModeType.PERCENT_ADD)
            { 
                percent += modifier.Value;  
            }
        }

        isDirty = false;

        return value *= (1 + percent);
    }
}
