
using UnityEditor.Rendering;
using UnityEngine;

namespace PlayerOwnedStates
{
    public class IdleState : State<Player>
    {
        public override void Enter(Player entity)
        {
            entity.Animator.SetBool("Walk", false);
            entity.Animator.SetBool("Idle", true);
        }

        public override void Execute(Player entity)
        {
            if((Vector3)Managers.Input.MoveInput != Vector3.zero)
            {
                entity.ChangeState(UnitState.Move);
            }
            else
            {
                Debug.Log("대기 상태입니다.");
            }

            // 공격상태가 되면 공격상태로 변경
        }

        public override void Exit(Player entity)
        {

        }

        public override bool OnMessage(Player entity, Telegram telegram)
        {
            return false;
        }
    }

    public class BattleState : State<Player>
    {
        public override void Enter(Player entity)
        {
            
        }

        public override void Execute(Player entity)
        {
            
        }

        public override void Exit(Player entity)
        {

        }

        public override bool OnMessage(Player entity, Telegram telegram)
        {
            return false;
        }
    }

    public class MoveState : State<Player>
    {
        private float stepSfxTime = 0f;
        public override void Enter(Player entity)
        {
            entity.Animator.SetBool("Walk", true);
            entity.Animator.SetBool("Idle", false);
        }

        public override void Execute(Player entity)
        {
            Vector3 input = Managers.Input.MoveInput;

            // 이동 X
            if (input.y <= 0)
            {
                entity.ChangeState(UnitState.Idle);
                return;
            }

            // 발소리
            if (stepSfxTime >= 0.05f)
            {
                int randStep = Random.Range(0, 2);
                string stepSfx = randStep == 0 ? KeyDefines.AudioKey.STEP_SFX1 : KeyDefines.AudioKey.STEP_SFX2;
                Managers.SoundManager.PlaySFX(stepSfx);
            }

            stepSfxTime += Time.deltaTime;

            // 이동
            Vector3 moveDirection = new Vector3(input.x, 0, input.y);
            Managers.Game.InGame.AddMoveDistance();
            entity.transform.Translate(moveDirection * entity.moveSpeed * Time.deltaTime);
        }

        public override void Exit(Player entity)
        {
            
        }

        public override bool OnMessage(Player entity, Telegram telegram)
        {
            return false;
        }
    }

    public class DieState : State<Player>
    {
        public override void Enter(Player entity)
        {
            // 죽는 소리 재생하기
            Managers.Battle.ChangeState(EBattleState.GameOver);
        }

        public override void Execute(Player entity)
        {



        }

        public override void Exit(Player entity)
        {

        }

        public override bool OnMessage(Player entity, Telegram telegram)
        {
            return false;
        }
    }

    public class StopState : State<Player>
    {
        public override void Enter(Player entity)
        {
            
        }

        public override void Execute(Player entity)
        {

        }

        public override void Exit(Player entity)
        {

        }

        public override bool OnMessage(Player entity, Telegram telegram)
        {
            return false;
        }
    }

}