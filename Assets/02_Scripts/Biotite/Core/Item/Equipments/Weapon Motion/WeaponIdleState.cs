using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class WeaponIdleState : WeaponStateBase
{
    private int currentAttackMotionIndex = 0;
    private int maxAttackMotionIndex;

    public WeaponIdleState(Player player, WeaponBase weapon, WeaponStateMachine stateMachine, Animator animator, string animName) : base(player, weapon, stateMachine, animator, animName)
    {
    }


    public override void Enter()
    {
        base.Enter();
        currentAttackMotionIndex = weapon.CurrentAttackMotionIndex;
        maxAttackMotionIndex = weapon.MaxAttackMotionIndex;
    }


    public override void Update()
    {
        if (Managers.Battle.currentState == EBattleState.Battle)
        {
            if (Managers.Input.AttackButton)
            {
                Attack();
            }
            if (Managers.Input.ParryingButton)
            {
                Parrying();
            }
        }
    }


    private void Attack()
    {
        WeaponAttackState currentAttack = weapon.AttackStates[currentAttackMotionIndex];
        stateMachine.ChangeState(currentAttack);
        weapon.CurrentAttackMotionIndex = (currentAttackMotionIndex + 1) % maxAttackMotionIndex;
    }


    //idle 상태일때 패링 모션 시작
    private void Parrying()
    {
        stateMachine.ChangeState(weapon.ParryingState);
    }


}
