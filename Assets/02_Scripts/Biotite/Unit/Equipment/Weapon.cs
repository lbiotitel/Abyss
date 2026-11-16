using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;



public class WeaponBase : EquipmentBase
{
    //데이터
    public WeaponData WeaponData { get; private set; }
    private Animator handAnimator;


    //FSM
    private WeaponStateMachine stateMachine;
    public WeaponIdleState IdleState { get; private set; }
    public WeaponParryingState ParryingState { get; private set; }
    public List<WeaponAttackState> AttackStates { get; private set; }
    private GameObject parryingParticlePrefab;
    private Transform parryingPos;

    //attack motion Index
    public int MaxAttackMotionIndex { get; private set; }
    public int CurrentAttackMotionIndex { get; set; }


    //무기를 초기화 합니다.
    public override void Init(Player player,ItemObject itemObject)
    {
        base.Init(player, itemObject);
        WeaponData = equipmentData as WeaponData;

        if (WeaponData.WeaponMotionData == null)
        {
            UnEquip();
            Logger.LogError($"{equipmentData.ItemName}에 무기 모션이 등록되지 않았습니다.");
            return;
        }
        handAnimator.runtimeAnimatorController = WeaponData.WeaponMotionData.WeaponAnimator;

        AnimationEventNotifier notifire = handAnimator.GetComponent<AnimationEventNotifier>();
        notifire.OnAnimTrigger += HandleAnimTrigger;

        InitParrying();
        InitStateMachine();
    }


    //패링에 필요한 이펙트, 이펙트 위치 이벤트 바인딩 등을 합니다.
    private void InitParrying()
    {
        parryingPos = new GameObject(WeaponData.ItemName + "ParryingPos").transform;
        Transform modelTr = transform.GetChild(0);
        parryingPos.SetParent(modelTr);
        parryingPos.localPosition = WeaponData.ParryingPos;
        parryingParticlePrefab = Managers.Resource.GetAsset<GameObject>(KeyDefines.ResourceKey.PARRYING_PARTICLE);
        Managers.Event.AddEvent<OnParryingSucceed>(HandleOnParryingSuccedEvent);
    }


    public void InitHandAnimator(Animator handAnimator)
        => this.handAnimator = handAnimator;



    //패링이 성공 됬을 때 이펙트 생성
    private void HandleOnParryingSuccedEvent(OnParryingSucceed e)
    {
        GameObject particle = Managers.Object.PoolingSystem.GetObject(parryingParticlePrefab, parryingPos.position, Quaternion.identity);
        particle.transform.localScale = Vector3.one * 0.4f;
    }


    //무기의 상태 머신을 초기화 합니다.
    private void InitStateMachine()
    {
        stateMachine = new WeaponStateMachine();

        IdleState = new WeaponIdleState(player, this, stateMachine, handAnimator, WeaponData.WeaponMotionData.IdleAnimName);
        ParryingState = new WeaponParryingState(player, this, stateMachine, handAnimator, WeaponData.WeaponMotionData.ParryingAnimName);

        InitAttackStates();

        stateMachine.InitState(IdleState);
    }


    private void InitAttackStates()
    {
        //공격에 관한 모든 애니메이션 상태를 만들고 순서대로 저장합니다.
        AttackStates = new List<WeaponAttackState>();
        string[] attackAnimes = WeaponData.WeaponMotionData.AtkAnimNames;
        MaxAttackMotionIndex = attackAnimes.Length;
        CurrentAttackMotionIndex = 0;

        if (attackAnimes == null || attackAnimes.Length == 0)
        {
            Logger.LogError($"{WeaponData.ItemName}에 등록된 공격 모션이 없습니다.");
            return;
        }

        for (int i = 0; i < attackAnimes.Length; i++)
        {
            WeaponAttackState weaponAttackState = new WeaponAttackState(player, this, stateMachine, handAnimator, attackAnimes[i]);
            AttackStates.Add(weaponAttackState);
        }
    }


    private void Update()
        => stateMachine?.UpdateState();



    //실제 애니메이션이 공격 중 데미지를 줄 수  있을 때 발동 됩니다.
    private void HandleAnimTrigger(EAnimationTriggerType animationTriggerType)
     => stateMachine.CurrentState?.AnimationFishedTrigger(animationTriggerType);



    //장비 해제
    public override void UnEquip()
    {
        AnimationEventNotifier notifire = handAnimator.GetComponent<AnimationEventNotifier>();
        notifire.OnAnimTrigger -= HandleAnimTrigger;
        handAnimator.runtimeAnimatorController = null;
        Managers.Event.RemoveEvent<OnParryingSucceed>(HandleOnParryingSuccedEvent);
        base.UnEquip();
    }


    private void OnDestroy()
    {
        AnimationEventNotifier notifire = handAnimator.GetComponent<AnimationEventNotifier>();
        notifire.OnAnimTrigger -= HandleAnimTrigger;
        Managers.Event.RemoveEvent<OnParryingSucceed>(HandleOnParryingSuccedEvent);
    }


    //패링이 가능한지 여부를 리턴 합니다.
    public bool IsParrying()
    {
        if (stateMachine.CurrentState == ParryingState)
        { 
            WeaponParryingState parryingState = stateMachine.CurrentState as WeaponParryingState;
            return parryingState.CanParrying; 
        }

        return false;
    }

}
