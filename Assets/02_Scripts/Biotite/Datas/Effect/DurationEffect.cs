using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;



//지속시간 동안 효과를 적용하는 이펙트 입니다.
[Serializable]
public class DurationEffect : EffectExcution
{
    [Header("Duration Effect Properties")]
    [SerializeField] private float duration;
    [SerializeField] private float tick;

    [SerializeReference,SubclassSelector] private List<EffectBase> subEffects;
    private bool isStop = false;

    public override void ExcuteEffect(Unit source, Unit target)
    {
        isStop = false;
        Run(source, target).Forget();
    }


    protected virtual async UniTaskVoid Run(Unit source, Unit target)
    {
        float t = 0;
        float tickT = 0;
        
        foreach (var effect in subEffects)
        {
            effect.Excute(source, target);
        }

        while (t <= duration)
        {
            if (isStop) break;
            tickT += Time.deltaTime;
            if (tickT >= tick)
            {
                foreach (var effect in subEffects)
                {
                    effect.Excute(source, target);
                }
                tickT = 0;
            }
            t += Time.deltaTime;
            await UniTask.Yield();
        }

        RemoveEffect();
    }

    protected virtual void RemoveEffect()
    {
        foreach (var effect in subEffects)
        {
            effect.StopEffect();
        }
    }

    public override void StopEffect()
    {
        isStop = true;
    }
}
