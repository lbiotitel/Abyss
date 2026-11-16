using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[Serializable]
public class PoisionEffect : EffectBase
{
    [Header("Poison Properties")]
    [SerializeField] private GameObject poisionParticle;
    [SerializeField] private float duration;
    [SerializeField] private float tick;
    [SerializeField] private int tickDamage;
    [SerializeField] private int maxStack;

    private Dictionary<Unit, int> currentStackDict = new Dictionary<Unit, int>();
    private Dictionary<Unit, CancellationTokenSource> uniTaskDict = new Dictionary<Unit, CancellationTokenSource>();

    public override void Excute(Unit source, Unit target)
    {
        if (currentStackDict.TryGetValue(target, out int stack))
        {
            currentStackDict[target] = Mathf.Min(stack + 1, maxStack);
        }
        else
        {
            currentStackDict[target] = 1;
        }

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

                if (currentStackDict.TryGetValue(target, out int currentStack))
                { 
                    GameObject.Instantiate(poisionParticle, target.transform.position, Quaternion.identity);
                    StatModifire statModifire = new StatModifire(-currentStack * tickDamage, EStatModeType.ADD, this);
                    target.UnitStats.AddModifire(EStatType.CurrentHP, statModifire);
                }
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
        if (currentStackDict.ContainsKey(target)) currentStackDict.Remove(target);

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
        currentStackDict.Clear();
    }
}