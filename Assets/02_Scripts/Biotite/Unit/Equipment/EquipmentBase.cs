using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//장비의 기본 부모 클래스입니다.
public class EquipmentBase : MonoBehaviour
{
    protected Player player;
    protected ItemObject itemObject;
    protected EquipmentData equipmentData;
    public EquipmentData EquipmentData => equipmentData;

    public virtual void Init(Player player, ItemObject itemObject)
    {
        this.player = player;
        this.itemObject = itemObject;
        equipmentData = itemObject.ItemData as EquipmentData;
    }


    public virtual void Equip()
    {
        List<SStatContainer> statContainers = equipmentData.StatContainers;

        if (statContainers == null || statContainers.Count == 0)
            return;

        foreach (var stat in statContainers)
        {
            StatModifier statModifier = new StatModifier(stat.value,stat.modeType,this);
            player.UnitStats.AddModifier(stat.statType,statModifier);
        }
    }


    public virtual void UnEquip()
    {
        List<SStatContainer> statContainers = equipmentData.StatContainers;

        foreach (var stat in statContainers)
        {
            StatModifier statModifier = new StatModifier(stat.value, stat.modeType, this);
            player.UnitStats.RemoveModifier(stat.statType, statModifier);
        }
    }
}
