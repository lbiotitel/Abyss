//이벤트에서 사용하는 데이터를 정의하는 곳입니다.

using System.Collections.Generic;
using UnityEngine;

public interface IEventData { }



//장비를 장착할 때
public struct OnEquippedEquipment : IEventData 
{
    public ItemObject InventoryItem;
}

public struct OnUnEquippedEquipment : IEventData
{
    public EEquipmentType equipmentType;
}



//플레이어가 공격, 피격, 패링에 성공했을 때 발생하는 이벤트입니다.
public struct OnAttack : IEventData { }
public struct OnPlayerHit : IEventData { }
public struct OnParryingSucceed : IEventData { }


//인게임이 시작될 때 발생하는 이벤트입니다.
public struct OnInGameStart : IEventData { }


public struct OnNextRoom : IEventData 
{ 
    public int moveDir;
}

public struct OnSpawnUnit : IEventData 
{ 
    public List<Unit> unitList;
}

public struct OnReward : IEventData { public List<string> rewards; }

public struct OnInGameOver : IEventData {}


//인벤토리 슬롯 마우스 엔터 엑시트
public struct OnItemSlotMouseEntered : IEventData
{
    public ItemDataBase itemData;
    public RectTransform rectTransform;
}

public struct OnItemSlotMouseExited : IEventData{}