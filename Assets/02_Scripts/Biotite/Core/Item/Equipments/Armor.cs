using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//방어구에 대한 정의 입니다.
public class Armor : EquipmentBase
{

    private ArmorData armorData;

    public override void Init(Player player, ItemObject itemObject)
    {
        base.Init(player, itemObject);
        armorData = equipmentData as ArmorData;
    }


    
}
