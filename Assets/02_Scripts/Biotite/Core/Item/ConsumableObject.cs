using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//소모품 아이템의 사용을 정의합니다.
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
        Logger.Log($"{consumableData.ItemName}을(를) 사용했습니다.");
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
