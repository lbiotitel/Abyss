using UnityEngine;

namespace EnemyOwnedStates
{
    public class IdleState : State<Enemy>
    {
        private float coolTime;

        public override void Enter(Enemy entity)
        {
            // UI 표시
            entity.Mark.SetActive(false);
            
            entity.Animator.SetBool("idle", true);
            entity.Animator.SetBool("attack", false);

            coolTime = entity.UnitStats.GetStatValue(EStatType.AtkSpeed);
        }

        public override void Execute(Enemy entity)
        {
            if (Managers.Battle.currentState != EBattleState.Battle) return;

            coolTime -= Time.deltaTime;

            if(coolTime <= 0) entity.ChangeState(UnitState.Attack);
        }

        public override void Exit(Enemy entity)
        {
        }

        public override bool OnMessage(Enemy entity, Telegram telegram)
        {
            return false;
        }
    }

    public class AttackState : State<Enemy>
    {
        public override void Enter(Enemy entity)
        {

            entity.Animator.SetBool("idle", false);
            entity.Animator.SetBool("attack", true);
        }

        public override void Execute(Enemy entity)
        {
            
        }

        public override void Exit(Enemy entity)
        {

        }

        public override bool OnMessage(Enemy entity, Telegram telegram)
        {
            return false;
        }
    }

    public class DieState : State<Enemy>
    {
        public override void Enter(Enemy entity)
        {
            entity.Animator.SetTrigger("die");
        }

        public override void Execute(Enemy entity)
        {



        }

        public override void Exit(Enemy entity)
        {

        }

        public override bool OnMessage(Enemy entity, Telegram telegram)
        {
            // 죽었다는 메세지를 받으면 뭐를 해야 되지?
            return false;
        }
    }
}