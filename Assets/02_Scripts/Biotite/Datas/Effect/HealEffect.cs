using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//회복효과에 대한 구현 입니다.
[Serializable]
public class HealEffect : EffectBase
{
    [SerializeField] private int healAmount;

    public override void Excute(Unit source, Unit target)
    {
       
    }


    public override void StopEffect()
    {
        
    }
}
