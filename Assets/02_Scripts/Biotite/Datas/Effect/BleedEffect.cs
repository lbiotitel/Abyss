using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


[Serializable]
public class BleedEffect : EffectBase
{
    [Header("BleedEffect Properties")]
    [SerializeField] private GameObject bleedParticle;
    [SerializeField] private float bleedDamage = 5;
    [SerializeField] private float duration;

    private Dictionary<Unit, CancellationTokenSource> uniTaskDict = new Dictionary<Unit, CancellationTokenSource>();


    public override void Execute(Unit source, Unit target)
    {
        if (target.IsDead) return;
        if (uniTaskDict.TryGetValue(target, out var oldUnitask))
        {
            oldUnitask.Cancel();
            oldUnitask.Dispose();
        }

        var newUnitask = new CancellationTokenSource();
        uniTaskDict[target] = newUnitask;

        Bleed(target, duration, newUnitask.Token).Forget();
    }


    //출혈효과 실행
    private async UniTask Bleed(Unit target, float time, CancellationToken cancellationToken)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: cancellationToken);

        if (target.IsDead) return;
        GameObject.Instantiate(bleedParticle, target.transform.position, Quaternion.identity);
        float damage = target.UnitStats.GetStatValue(EStatType.MaxHp) * (bleedDamage / 100f);
        Managers.Object.CreateDamageText(damage.ToString(), EDamageTextType.BLEED,target.transform.position);
        target.TakeDamage(null,-damage);

        ClearTarget(target);
    }


    //특정 유닛의 저장된 유니태스크 제거
    private void ClearTarget(Unit target)
    {
        if (uniTaskDict.TryGetValue(target, out var cts))
        {
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
