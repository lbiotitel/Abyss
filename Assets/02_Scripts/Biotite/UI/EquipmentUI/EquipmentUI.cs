using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.UIElements;


//장비 ui 입니다.
public class EquipmentUI : MonoBehaviour
{
    [SerializeField] private EquipmentSlotUI[] equipmentSlotUIs;

    private Dictionary<EEquipmentType, ItemObject> equipmentDict;

    private void Start()
    {
        Init();
        equipmentDict = new Dictionary<EEquipmentType, ItemObject>();
    }


    private void Init()
    {
        foreach (var slot in equipmentSlotUIs)
        {
            slot.OnClickedEquipmentSlotUI += HandleEquippedEquipment;
        }
    }

    private void OnDestroy()
    {
        foreach (var slot in equipmentSlotUIs)
        {
            slot.OnClickedEquipmentSlotUI -= HandleEquippedEquipment;
        }
    }



    //아이템 장착 스탯올리는 로직 3가지 경우
    private void HandleEquippedEquipment(EquipmentSlotUI equipmentSlotUI, InventoryItemSlotUI mouseItemSlotUI)
    {
        ItemObject mouseItem = mouseItemSlotUI != null ? mouseItemSlotUI.itemObject : null;

        bool hasCurrentEquipment = equipmentDict.TryGetValue(equipmentSlotUI.EquipmentType, out ItemObject currentEquipment);

        if (hasCurrentEquipment)
        {
            EquipmentData currentEData = currentEquipment.ItemData as EquipmentData;

            if (mouseItem == null)
            {
                //마우스에 템이 없고 클릭 했을 시 장착 해제
                Logger.Log($"{currentEData.ItemName} 장착 해제");

                InventoryItemSlotUI inventoryItemSlotUI = Managers.Object.CreateInventoryItem(currentEData);

                MouseObject.Instance.AssignItem(inventoryItemSlotUI);
                equipmentSlotUI.UnEquip();
                equipmentDict.Remove(equipmentSlotUI.EquipmentType);

                Managers.Event.NotifyEvent<OnUnEquippedEquipment>(new OnUnEquippedEquipment { equipmentType = currentEData.EquipmentType });
            }

            else
            {
                //마우스에 템이 있고 장착까지 하고있을 때 스왑
                if (mouseItem.ItemData.ItemType != EItemType.Equipment) return;
                EquipmentData mouseEData = mouseItem.ItemData as EquipmentData;

                if (equipmentSlotUI.EquipmentType != mouseEData.EquipmentType) return;

                Managers.Event.NotifyEvent<OnUnEquippedEquipment>(new OnUnEquippedEquipment { equipmentType = currentEData.EquipmentType });

                equipmentDict[equipmentSlotUI.EquipmentType] = mouseItem;
                equipmentSlotUI.Equip(mouseEData);

                InventoryItemSlotUI currentEquipmentSlotUi = Managers.Object.CreateInventoryItem(currentEData);
                MouseObject.Instance.ClearItem();
                MouseObject.Instance.AssignItem(currentEquipmentSlotUi);
                Logger.Log($"{currentEData.ItemName} 해제 및 {mouseEData.ItemName} 교체 장착");

                Managers.Event.NotifyEvent<OnEquippedEquipment>(new OnEquippedEquipment { InventoryItem = mouseItem });
            }
            return;
        }


        //마우스에 템이있고 현재 장착된 아이템이 없을 때 그냥 장착
        if (mouseItem == null) return;
        if (mouseItem.ItemData.ItemType != EItemType.Equipment) return;

        EquipmentData newEquipmentData = mouseItem.ItemData as EquipmentData;

        if (equipmentSlotUI.EquipmentType != newEquipmentData.EquipmentType) return;

        Logger.Log($"{newEquipmentData.ItemName} 새로 장착");

        equipmentDict.Add(equipmentSlotUI.EquipmentType, mouseItem);
        equipmentSlotUI.Equip(newEquipmentData);
        Managers.Event.NotifyEvent<OnEquippedEquipment>(new OnEquippedEquipment { InventoryItem = mouseItem });
        MouseObject.Instance.ClearItem();
    }



}
