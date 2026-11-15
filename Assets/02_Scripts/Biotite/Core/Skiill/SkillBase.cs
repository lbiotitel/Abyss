using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;



//스킬로직에 관한 정의 입니다.
public class SkillBase
{
    private Player player;
    public SkillData SkillData { get; private set; }
    private bool isCoolTime = false;

    public event Action<SkillBase, float> OnSkillActive;

    public void Init(Player palyer, SkillData skillData)
    {
        this.player = palyer;
        this.SkillData = skillData;
        InitEvent();
    }


    //데이터를 트리거에 따라 이벤트을 바인딩 합니다.
    private void InitEvent()
    {
        ESkillTrigger skillTrigger = SkillData.SkillTrigger;

        if (skillTrigger == ESkillTrigger.ATTACK)
        {
            Managers.Event.AddEvent<OnAttack>(DoSkill);
        }
        else if (skillTrigger == ESkillTrigger.PARRYING)
        {
            Managers.Event.AddEvent<OnParryingSucceed>(DoSkill);
        }
        else if (skillTrigger == ESkillTrigger.ONHIT)
        {
            Managers.Event.AddEvent<OnPlayerHit>(DoSkill);
        }
    }


    //스킬 해제
    public void Release()
    {
        Managers.Event.RemoveEvent<OnAttack>(DoSkill);
        Managers.Event.RemoveEvent<OnParryingSucceed>(DoSkill);
        Managers.Event.RemoveEvent<OnPlayerHit>(DoSkill);
    }



    //스킬 이벤트에 따라 스킬 사용 호출

    private void DoSkill(OnAttack e) => DoSkill();
    private void DoSkill(OnParryingSucceed e) => DoSkill();
    private void DoSkill(OnPlayerHit e) => DoSkill();


    //스킬을 실행
    private void DoSkill()
    {
        if (isCoolTime || !CheckManaCost()) return;

        CoolTime(player.GetCancellationTokenOnDestroy()).Forget();

        isCoolTime = true;
        OnSkillActive?.Invoke(this, SkillData.CoolTime);

        StatModifire mpModifire = new StatModifire(-SkillData.ManaCost, EStatModeType.ADD, this);
        player.UnitStats.AddModifire(EStatType.CurrentMP, mpModifire);

        List<Enemy> targetList = Managers.Battle.GetTarget(SkillData.SkillRange);

        foreach (var effect in SkillData.EffectList)
        {
            foreach (var target in targetList)
            {
                effect.ExcuteEffect(player, target);
            }
        }
    }



    //스킬 초기화
    public void ResetSkill()
    {
        isCoolTime = false;
        OnSkillActive?.Invoke(this, 0);
        foreach (var effect in SkillData.EffectList)
        {
            effect.StopEffect();
        }
    }


    //마나가 충분한지 확인합니다.
    private bool CheckManaCost()
    {
        float currentMp = player.UnitStats.GetStatValue(EStatType.CurrentMP);
        if (currentMp < SkillData.ManaCost)
        {
            Debug.Log("스킬 사용에 필요한 마나가 부족합니다.");
            return false;
        }

        return true;
    }



    //쿨타임
    private async UniTaskVoid CoolTime(System.Threading.CancellationToken cancellationToken)
    {
        bool isCanceled = await UniTask.WaitForSeconds(SkillData.CoolTime, cancellationToken: cancellationToken).SuppressCancellationThrow();

        if (!isCanceled)
        {
            isCoolTime = false;
        }
    }

}
