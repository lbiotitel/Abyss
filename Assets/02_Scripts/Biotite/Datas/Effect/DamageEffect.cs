using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//데미지 효과에 대한 구현 입니다.
[Serializable]
public class DamageEffect : EffectBase
{
    [SerializeField] private float damage;
    public override void Excute(Unit source, Unit target)
    {
        Logger.Log($"{source}가 데미지를 준다. {target} 에게");
        UnitStatContainer unitStat = source.UnitStats;

        target.TakeDamage(source);
        Managers.Object.CreateDamageText(unitStat.GetStatValue(EStatType.ATK).ToString(), EDamageTextType.NORMAL, target.transform.position);
    }


    public override void StopEffect()
    {
        
    }
}
