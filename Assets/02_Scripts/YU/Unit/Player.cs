using UnityEditor;
using UnityEngine;

public class Player : Unit
{
    [SerializeField] private CameraController cameraController;
    [SerializeField] private EquipmentSystem equipmentSystem;
    [SerializeField] PlayerData datas;
    private PlayerUI playerUI;
    private PlayerInfoUI playerInfoUI;

    public float moveSpeed { get => 4; set => moveSpeed = value; }

    // State
    private State<Player>[] states;
    private StateMachine<Player> stateMachine;

    public Vector3 _moveInput { get; private set; }
    public bool invincible = false; // 무적

    public override void Setup(UnitData data)
    {
        base.Setup(data);

        // 데이터 설정값 넣기
        PlayerData pdata = data as PlayerData;
        unitData = pdata;
        gameObject.name = pdata.name;


        states = new State<Player>[5];
        states[(int)UnitState.Idle] = new PlayerOwnedStates.IdleState();
        states[(int)UnitState.Move] = new PlayerOwnedStates.MoveState();
        states[(int)UnitState.Attack] = new PlayerOwnedStates.BattleState();
        states[(int)UnitState.Die] = new PlayerOwnedStates.DieState();
        states[(int)UnitState.Stop] = new PlayerOwnedStates.StopState();

        // 상태를 관리하는 StateMachine 만들고, 첫 설정
        stateMachine = new StateMachine<Player>();
        stateMachine.Setup(this, states[(int)UnitState.Idle]);
    }

    public override void Updated()
    {
        stateMachine.Execute();
    }

    public void ChangeState(UnitState newState)
    {
        currentState = newState;
        stateMachine.ChangeState(states[(int)newState]);
    }

    public override bool HandleMessage(Telegram telegram)
    {
        return stateMachine.HandleMessage(telegram);
    }

    //초기화
    public override void Init(UnitData data)
    {
        base.Init(data);
        equipmentSystem.Init(this);
        playerUI = GameObject.FindAnyObjectByType<PlayerUI>();
        playerUI.Init(unitStats);
        playerInfoUI = GameObject.FindAnyObjectByType<PlayerInfoUI>();
        playerInfoUI.Init(unitStats);
        cameraController.Init();
    }

    private void Update()
    {
        this.Updated();
    }


    public override void TakeDamage(Unit target)
    {
        if (equipmentSystem.Weapon.IsParrying())
        {
            Managers.SoundManager.PlaySFX(KeyDefines.AudioKey.PARRYING_SFX);
            Managers.Event.NotifyEvent<OnParryingSucceed>(new OnParryingSucceed { });
            return;
        }
        Managers.Event.NotifyEvent<OnPlayerHit>(new OnPlayerHit { });
        base.TakeDamage(target);
    }

    public override void TakeDamage(Unit target, float damage)
    {
        Managers.Event.NotifyEvent<OnPlayerHit>(new OnPlayerHit { });
        base.TakeDamage(target, damage);
    }

    public override void Die()
    {
        base.Die();
        ChangeState(UnitState.Die);
        stateMachine.StopStateMahine();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            Managers.Battle.ChangeState(EBattleState.Start);
        }
    }
}
