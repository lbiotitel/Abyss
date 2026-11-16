using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//무기의 공격 상태를 정의합니다. 
public class WeaponAttackState : WeaponStateBase
{
    protected bool attackAnimTrigger = false;

    public WeaponAttackState(Player player, WeaponBase weapon, WeaponStateMachine stateMachine, Animator animator, string animName) : base(player, weapon, stateMachine, animator, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        attackAnimTrigger = false;
        animFinishedTrigger = false;
        float currentAtkSpeed = player.UnitStats.GetStatValue(EStatType.AtkSpeed);
        animator.speed = currentAtkSpeed;
        Managers.SoundManager.PlaySFX(weapon.WeaponData.WeaponSFX);
    }

    public override void Exit()
    {
        base.Exit();
        animator.speed = 1;
    }

    public override void Update()
    {
        CheckAttack();
        if (animFinishedTrigger)
        {
            animFinishedTrigger = false;
            stateMachine.ChangeState(weapon.IdleState);
        }
    }

    
    public override void AnimationFinishedTrigger(EAnimationTriggerType animationTriggerType)
    {
        base.AnimationFinishedTrigger(animationTriggerType);
        if (animationTriggerType == EAnimationTriggerType.ATTACK) attackAnimTrigger = true;
    }


    //공격 타이밍을 체크합니다
    private void CheckAttack()
    {
        if (attackAnimTrigger)
        {
            attackAnimTrigger = false;
            //상대를 골라 넣으면 ehla
            Managers.Event.NotifyEvent<OnAttack>(new OnAttack {  });
        }
    }
}
