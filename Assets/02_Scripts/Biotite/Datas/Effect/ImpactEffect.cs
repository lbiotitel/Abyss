using System;
using System.Collections.Generic;
using UnityEngine;



//즉시 일어나는 이벤트 입니다.
[Serializable]
public class ImpactEffect : EffectExcution
{
    [SerializeReference, SubclassSelector] private List<EffectBase> subEffects;

    public override void ExcuteEffect(Unit source, Unit target)
    {
        foreach (var effect in subEffects)
        { 
            effect.Excute(source, target);   
        }
    }

    public override void StopEffect()
    {
        
    }
}
