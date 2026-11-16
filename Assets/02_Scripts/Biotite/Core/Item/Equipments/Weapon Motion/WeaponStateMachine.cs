using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//무기의 상태를 바꾸거나 초기화하는 클래스입니다.
public class WeaponStateMachine 
{

    public WeaponStateBase CurrentState { get; private set; }

    public void InitState(WeaponStateBase newWeaponState)
    { 
        CurrentState = newWeaponState;
        CurrentState?.Enter();
    }

    public virtual void ChangeState(WeaponStateBase newWeaponState)
    {
        CurrentState?.Exit();
        CurrentState = newWeaponState;
        CurrentState?.Enter();
    }

    public void UpdateState()
        => CurrentState?.Update();

    public void AnimTrigger(EAnimationTriggerType animationTriggerType)
        => CurrentState?.AnimationFinishedTrigger(animationTriggerType);
  
}
