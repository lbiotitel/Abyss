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
            StatModifire statModifire = new StatModifire(stat.value,stat.modeType,this);
            source.UnitStats.AddModifire(stat.statType, statModifire);
        }
    }

    public override void StopEffect()
    {
        if (source == null) return;
        foreach (var stat in statContainer)
        {
            StatModifire statModifire = new StatModifire(stat.value, stat.modeType, this);
            source.UnitStats.RemoveModifire(stat.statType, statModifire);
        }
    }
}
