using UnityEngine;

public enum UnitState { Idle, Attack, Move, Die, Stop }

public abstract class Unit : MonoBehaviour
{
    protected UnitData unitData;
    protected UnitStatContainer unitStats = new();
    private bool isDead = false;
    [SerializeField] protected Animator animator;

    public UnitStatContainer UnitStats => unitStats;
    public Animator Animator => animator;
    public bool IsDead => isDead;

    public UnitState currentState; // 현재 상태

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // 상태
    public virtual void Init(UnitData data)
    {
        unitStats = new UnitStatContainer();

        unitStats.Init(data);
    }

    public virtual void Setup(UnitData data)
    {
        gameObject.name = data.UnitName;
    }

    public abstract void Updated();

    // 메세지를 발송했을 때 수신하는 메소드(MessageDispatcher 클래스에서 호출)
    public abstract bool HandleMessage(Telegram telegram);

    public virtual void TakeDamage(Unit target)
    {
        // 회피 확률 - 민첩의 기반해서 회피확률을 구합니다.
        // 공식: 최종회피율 = 자신(회피율) + (자신(민첩) - 적(민첩)) * 상수1

        Managers.SoundManager.PlaySFX(KeyDefines.AudioKey.HIT_SFX);

        float DMG = unitStats.GetStatValue(EStatType.DEF) - target.unitStats.GetStatValue(EStatType.ATK)
;

        if (DMG > 0) DMG = 0;

        StatModifire hpModifire = new StatModifire(DMG, EStatModeType.ADD, this);

        unitStats.AddModifire(EStatType.CurrentHP, hpModifire);

        Debug.Log($"[{gameObject.name}업데이트]현재 HP: {unitStats.GetStatValue(EStatType.CurrentHP)}");

        if (unitStats.GetStatValue(EStatType.CurrentHP) <= 0)
        {
            Die();
        }
    }

    public virtual void TakeDamage(Unit target, float damage)
    {

        Managers.SoundManager.PlaySFX(KeyDefines.AudioKey.HIT_SFX);

        if (damage > 0) damage = 0;

        StatModifire hpModifire = new StatModifire(damage, EStatModeType.ADD, this);

        unitStats.AddModifire(EStatType.CurrentHP, hpModifire);

        Debug.Log($"[{gameObject.name}업데이트]현재 HP: {unitStats.GetStatValue(EStatType.CurrentHP)}");

        if (unitStats.GetStatValue(EStatType.CurrentHP) <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        if (isDead) return;
        isDead = true;
        Managers.SoundManager.PlaySFX(KeyDefines.AudioKey.DEAD_SFX);
    }
}
