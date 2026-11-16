using UnityEngine;

public class WeaponParryingState : WeaponStateBase
{
    protected bool parryingStartTrigger = false;
    protected bool parryingEndTrigger = false;

    public bool CanParrying { get; private set; } = false;

    public WeaponParryingState(Player player, WeaponBase weapon, WeaponStateMachine stateMachine, Animator animator, string animName) : base(player, weapon, stateMachine, animator, animName)
    {
    }


    public override void Enter()
    {
        base.Enter();
        parryingStartTrigger = false;
        parryingEndTrigger = false;
    }



    public override void AnimationFishedTrigger(EAnimationTriggerType animationTriggerType)
    {
        base.AnimationFishedTrigger(animationTriggerType);
        if (animationTriggerType == EAnimationTriggerType.PARRYING_START)
        {
            parryingStartTrigger = true;
            CanParrying = true;
            Logger.Log("패링 시작");
        }
        if (animationTriggerType == EAnimationTriggerType.PARRYING_END)
        {
            parryingEndTrigger = false;
            CanParrying = false;
            Logger.Log("패링 끝");
        }
    }



    public override void Update()
    {
        if (!Managers.Input.ParryingButton && animFinishedTrigger)
        {
            animFinishedTrigger = false;
            stateMachine.ChangeState(weapon.IdleState);
        }
    }
}
