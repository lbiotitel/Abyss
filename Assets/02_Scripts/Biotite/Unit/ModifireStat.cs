using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifireStat : StatBase
{

    private List<StatModifire> statModifires;
    private bool isDirty = true;
    private float cashedValue = 0;

    public override float GetValue()
    {
        if (isDirty) cashedValue = CalcModifire();
        return cashedValue;
    }


    public ModifireStat(float value, EStatType statType) : base(value, statType)
    {
        statModifires = new List<StatModifire>();
    }

    public override void AddModifire(StatModifire statModifire)
    {
        float prevValue = GetValue();
        statModifires.Add(statModifire);
        isDirty = true;
        TryInvokeOnValueChanged(prevValue, GetValue());
    }

    public override void RemoveModifire(StatModifire statModifire)
    {
        float prevValue = GetValue();
        statModifires.RemoveAll((s) => s.Resource == statModifire.Resource);
        isDirty = true;
        TryInvokeOnValueChanged(prevValue, GetValue());
    }


    private float CalcModifire()
    {
        float percent = 0;
        float value = baseValue;

        foreach (var modifire in statModifires)
        {
            if (modifire.StatModeType == EStatModeType.ADD)
            {
                value += modifire.Value;
            }
            else if (modifire.StatModeType == EStatModeType.PERCENT_ADD)
            { 
                percent += modifire.Value;  
            }
        }

        isDirty = false;

        return value *= (1 + percent);
    }
}
