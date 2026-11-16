using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;


[Serializable]
public class BuffEffect : EffectBase
{
    [SerializeField] List<SStatContainer> statContainer;
    private Unit source = null;

    public override void Excute(Unit source, Unit target)
    {
        this.source = source;
        foreach (var stat in statContainer)
        {
            StatModifier statModifier = new StatModifier(stat.value,stat.modeType,this);
            source.UnitStats.AddModifier(stat.statType, statModifier);
        }
    }

    public override void StopEffect()
    {
        if (source == null) return;
        foreach (var stat in statContainer)
        {
            StatModifier statModifier = new StatModifier(stat.value, stat.modeType, this);
            source.UnitStats.RemoveModifier(stat.statType, statModifier);
        }
    }
}
