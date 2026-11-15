using System.Collections.Generic;
using UnityEngine;

public enum EBattleState { None, Start, Battle, Exit, GameOver }


// 전투 관리를 하는 매니저
public class BattleManager : MonoBehaviour
{
    public EBattleState currentState;
    public event System.Action<EBattleState> OnStateChanged;

    private SpawnSystem spawnSystem;

    [SerializeField] private List<Unit> unitList;

    public void Init()
    {
        OnStateChanged += HandleBattleState;
        spawnSystem = new();
    }

    public void Setup(List<Unit> units)
    {
        // 생성된 유닛들의 리스트를 넣기
        foreach (Unit unit in units)
        {
            unitList.Add(unit);
        }
    }

    // 타겟 가져오기
    public List<Enemy> GetTarget(ESkillRange atkRange)
    {
        List<Enemy> list = new();

        foreach (Enemy e in unitList)
        {
            if (e.IsDead) continue;

            // 단일
            if (atkRange == ESkillRange.Single)
            {
                list.Add((Enemy)unitList[0]);
                break;
            }
            // 범위
            else if (atkRange == ESkillRange.Ranged)
            {
                list.Add(e);
            }
        }

        return list;
    }

    // 배틀 상태에 따라 해당 기능 실행하기
    public void HandleBattleState(EBattleState state)
    {
        switch (state)
        {
            case EBattleState.Start:
                // 플레이어의 이동막기
                Managers.Game.InGame.Player.ChangeState(UnitState.Attack);
                ChangeState(EBattleState.Battle);

                break;
            case EBattleState.Battle:
                // 배틀 속행

                break;
            case EBattleState.Exit:
                // 보상 드랍
                Managers.Game.InGame.Player.ChangeState(UnitState.Stop);
                List<string> selectedItems = Managers.Reward.GetRandomRewards(Random.Range(0, 4));

                Managers.Event.NotifyEvent<OnReward>(new OnReward
                {
                    rewards = selectedItems
                });

                break;

            case EBattleState.GameOver:
                unitList.Clear();
                Managers.Game.InGame.GameOver();
                break;
        }
    }

    // 배틀 상태 변경
    public void ChangeState(EBattleState state)
    {
        currentState = state;
        OnStateChanged?.Invoke(state);
    }

    public void RemoveTarget(Unit unit)
    {
        unitList.Remove(unit);

        if (unitList.Count <= 0)
        {
            ChangeState(EBattleState.Exit);
        }
    }
}
