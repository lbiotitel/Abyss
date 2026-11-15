using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//스킬 데이터를 정의합니다.

[CreateAssetMenu]
public class SkillData : ScriptableObject
{
    [Header("Skill Properties")]
    [SerializeField] private string skillName;
    [SerializeField,TextArea] private string skillDesc;
    [SerializeField] private Sprite skillIcon;
    [SerializeField] private float manaCost;
    [SerializeField] private float coolTime;
    [SerializeField] private ESkillTrigger skillTriggerType;
    [SerializeField] private ESkillRange skillRange;


    [Header("Effects")]
    [SerializeReference,SubclassSelector]
    private List<EffectExcution> effectList;


    public ESkillRange SkillRange => skillRange;
    public string SkillName => skillName;
    public string SkillDesc => skillDesc;
    public Sprite SkillIcon => skillIcon;
    public float CoolTime => coolTime;  
    public ESkillTrigger SkillTrigger => skillTriggerType;
    public float ManaCost => manaCost;
    public List<EffectExcution> EffectList => effectList;
}



//스킬이 발동할 트리거 정의 입니다.
public enum ESkillTrigger
{ 
    ATTACK, //공격할때
    PARRYING, //패링 할때
    ONHIT, //맞을때
}


//스킬 범위
public enum ESkillRange
{
    Single,
    Ranged
}