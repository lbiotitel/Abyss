using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;



[Serializable]
public class IgniteEffect : EffectBase
{
    [Header("Ignite Properties")]
    [SerializeField] private GameObject igniteParticle;
    [SerializeField] private float duration;
    [SerializeField] private float tickDamage = 10;
    [SerializeField] private float tick;

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

        LoopPoison(target, duration, tick, newUnitask.Token).Forget();
    }


    //독 틱 데미지 실행
    private async UniTask LoopPoison(Unit target, float time, float tickTime, CancellationToken cancellationToken)
    {
        float elapsedTime = 0f;

        try
        {
            while (elapsedTime < time)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(tickTime), cancellationToken: cancellationToken);
                elapsedTime += tickTime;

                Managers.Object.PoolingSystem.GetObject(igniteParticle, target.transform.position, Quaternion.identity);
                if (target.IsDead)
                {
                    ClearTarget(target);
                    return;
                }
                Managers.Object.CreateDamageText(tickDamage.ToString(), EDamageTextType.IGNITE, target.transform.position);
                target.TakeDamage(null,-tickDamage);
            }
        }
        catch (OperationCanceledException)
        {
            return;
        }

        ClearTarget(target);
    }


    //특정 유닛의 독중첩과 유니태크스 중지
    private void ClearTarget(Unit target)
    {
        if (uniTaskDict.TryGetValue(target, out var cts))
        {
            cts.Cancel();
            cts.Dispose();
            uniTaskDict.Remove(target);
        }
    }


    //독 중첩과 가지고 있는 딕셔너리 모두 제거
    public override void StopEffect()
    {
        foreach (var cts in uniTaskDict.Values)
        {
            cts.Cancel();
            cts.Dispose();
        }
        uniTaskDict.Clear();
    }
}
