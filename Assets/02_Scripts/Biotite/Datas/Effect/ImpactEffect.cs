using System;
using System.Collections.Generic;
using UnityEngine;



//즉시 일어나는 이벤트 입니다.
[Serializable]
public class ImpactEffect : EffectExecution
{
    [SerializeReference, SubclassSelector] private List<EffectBase> subEffects;

    public override void ExecuteEffect(Unit source, Unit target)
    {
        foreach (var effect in subEffects)
        { 
            effect.Execute(source, target);   
        }
    }

    public override void StopEffect()
    {
        
    }
}
