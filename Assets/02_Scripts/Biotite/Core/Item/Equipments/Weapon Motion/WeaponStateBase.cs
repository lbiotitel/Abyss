using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//애니메이션 트리거 타입을 정의합니다.
public enum EAnimationTriggerType
{ 
    FINISH,
    ATTACK,
    PARRYING_START,
    PARRYING_END
}


//무기의 FSM 상태를 정의합니다.
public class WeaponStateBase
{
    protected Player player;
    protected Animator animator;
    protected WeaponBase weapon;
    protected WeaponStateMachine stateMachine;
    protected string animName;

    protected bool animFinishedTrigger = false;


    public WeaponStateBase(Player player, WeaponBase weapon ,WeaponStateMachine stateMachine, Animator animator, string animName)
    {
        this.player = player;
        this.animator = animator;
        this.weapon = weapon;
        this.stateMachine = stateMachine;
        this.animName = animName;
    }

    public virtual void AnimationFishedTrigger(EAnimationTriggerType animationTriggerType)
    {
        if(animationTriggerType == EAnimationTriggerType.FINISH)
            animFinishedTrigger = true;
    }


    public virtual void Enter()
    {
        animFinishedTrigger = false;
        animator.SetBool(animName, true);
    }

    public virtual void Update()
    { 
       
    }

    public virtual void Exit() 
    {
        animator.SetBool(animName,false);
    }

}
