using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[Serializable]
public class FrozenEffect : EffectBase
{
    [Header("FrozenEffect Properties")]
    [SerializeField] private GameObject frozenParticle;
    [SerializeField] private float atkSpeedDecrease = 30;
    [SerializeField] private float duration;

    private Dictionary<Unit, CancellationTokenSource> uniTaskDict = new Dictionary<Unit, CancellationTokenSource>();


    public override void Excute(Unit source, Unit target)
    {
        if (uniTaskDict.TryGetValue(target, out var oldUnitask))
        {
            oldUnitask.Cancel();
            oldUnitask.Dispose();
        }

        var newUnitask = new CancellationTokenSource();
        uniTaskDict[target] = newUnitask;

        Frozen(target, duration, newUnitask.Token).Forget();
    }


    //빙결 실행
    private async UniTask Frozen(Unit target, float time, CancellationToken cancellationToken)
    {
        GameObject.Instantiate(frozenParticle, target.transform.position, Quaternion.identity);
        StatModifier statModifier = new StatModifier(-atkSpeedDecrease, EStatModeType.PERCENT_ADD, this);
        target.UnitStats.AddModifier(EStatType.AtkSpeed, statModifier);

        await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: cancellationToken);

        ClearTarget(target);
    }


    //특정 유닛의 공격속도 원복과 저장된 유니태스크 제거
    private void ClearTarget(Unit target)
    {
        if (uniTaskDict.TryGetValue(target, out var cts))
        {
            StatModifier statModifier = new StatModifier(atkSpeedDecrease, EStatModeType.PERCENT_ADD, this);
            target.UnitStats.AddModifier(EStatType.AtkSpeed, statModifier);
            cts.Cancel();
            cts.Dispose();
            uniTaskDict.Remove(target);
        }
    }


    //가지고 있는 딕셔너리 모두 제거
    public override void StopEffect()
    {
        foreach (var cts in uniTaskDict)
        {
            ClearTarget(cts.Key);
        }
        uniTaskDict.Clear();
    }
}