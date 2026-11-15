using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//영지로 귀환 이펙트
[Serializable]
public class ReturnEffect : EffectBase
{

    public override void Excute(Unit source, Unit target)
    {
        if (Managers.Battle.currentState == EBattleState.Battle) return;
        Managers.Game.InGame.GameOver();        
    }

    public override void StopEffect(){}
}
