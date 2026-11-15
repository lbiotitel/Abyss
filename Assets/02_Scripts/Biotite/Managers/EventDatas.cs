//�̺�Ʈ���� �����͸� ����ϴ� ���Դϴ�.

using System.Collections.Generic;
using UnityEngine;

public interface IEventData { }



//��� ������ ��
public struct OnEquippedEquipment : IEventData 
{
    public ItemObject InventoryItem;
}

public struct OnUnEquippedEquipment : IEventData
{
    public EEquipmentType equipmentType;
}



//�÷��̾ ������ �� �߻��ϴ� �̺�Ʈ�Դϴ�.
public struct OnAttack : IEventData { }
public struct OnPlayerHit : IEventData { }
public struct OnParryingSucceed : IEventData { }


//�� ������ ���۵ɶ� �߻��ϴ� �̺�Ʈ�Դϴ�.
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