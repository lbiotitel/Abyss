using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�Ҹ�ǰ�����ۿ� ���� �����Դϴ�.
public class ConsumableObject : ItemObject
{
    private ConsumableData consumableData;

    public override void Init(ItemDataBase itemData,InventoryItemSlotUI inventoryItemSlotUI)
    {
        base.Init(itemData, inventoryItemSlotUI);
        consumableData = itemData as ConsumableData;
    }


    public override void UseItem()
    {
        Logger.Log($"{consumableData.ItemName}�� ����߽��ϴ�.");
        Player player = Managers.Game.InGame.Player;
        foreach(var effect in consumableData.EffectExcutions)
        {
            effect.ExcuteEffect(player,null);
        }

        Managers.Event.NotifyEvent<OnItemSlotMouseExited>(new OnItemSlotMouseExited { });

        if (consumableData.IsPermanent) return;
        GameObject.Destroy(slotUI.gameObject);
    }
    
}
