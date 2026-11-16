using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentData : ItemDataBase
{
    [Header("Equipment Properties")]
    [SerializeField] protected EEquipmentType equipmentType;

    [Header("Stat Modifier")]
    [SerializeField] private List<SStatContainer> statContainers;

    [Header("Skills")]
    [SerializeField] private List<SkillData> skills;

    public EEquipmentType EquipmentType => equipmentType;
    public List<SStatContainer> StatContainers => statContainers;
    public List<SkillData> SkillDatas => skills;
}


[Serializable]
public struct SStatContainer
{
    public float value;
    public EStatType statType;
    public EStatModeType modeType;
}


public enum EEquipmentType
{
    WEAPON,
    HELMET,
    ARMOR,
    PANTS,
    BOOTS,
    GLOVE,
    SHIELD
}
