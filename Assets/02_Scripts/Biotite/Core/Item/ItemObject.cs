using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//인벤토리 UI에 있는 아이템 로직을 담당합니다.
public class ItemObject 
{
    protected ItemDataBase itemData;
    protected InventoryItemSlotUI slotUI;

    public ItemDataBase ItemData => itemData;



    //아이템데이터를 가지고 초기화
    public virtual void Init(ItemDataBase itemData, InventoryItemSlotUI slotUI)
    {
        this.itemData = itemData;
        this.slotUI = slotUI;
    }


    //인벤토리에서 아이템을 사용했을 때를 정의합니다
    public virtual void UseItem() { }
}
