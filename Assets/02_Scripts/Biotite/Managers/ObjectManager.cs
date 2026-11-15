using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] private InventoryItemSlotUI inventoryItemPrefab;

    [SerializeField] private GameObject playerPefab;

    [SerializeField] private DamageText damageTextPrefab;

    public PoolingSystem PoolingSystem { get; private set; }

    public void Init()
    {
        PoolingSystem = GetComponentInChildren<PoolingSystem>();
        PoolingSystem.Init();   
    }


    //데미지 텍스트를 생성 합니다.
    public void CreateDamageText(string msg, EDamageTextType damageTextType, Vector3 position)
    {
        GameObject poolingObject = Managers.Object.PoolingSystem.GetObject(damageTextPrefab.gameObject,position ,Quaternion.identity);
        DamageText damageText = poolingObject.GetComponent<DamageText>();
        damageText.transform.position = position + new Vector3(0, 0.5f, -1f);
        damageText.Init(msg, damageTextType);
    }


    //인벤토리 아이템의 생성을 위임 받아 생성합니다.
    public InventoryItemSlotUI CreateInventoryItem(ItemDataBase itemData)
    {
        InventoryItemSlotUI slotUI = Instantiate(inventoryItemPrefab);

        ItemObject item = null;
        if (itemData.ItemType == EItemType.Consumable)
        {
            item = new ConsumableObject();
            item.Init(itemData, slotUI);
        }
        else if (itemData.ItemType == EItemType.Equipment)
        { 
            item = new ItemObject();
            item.Init(itemData, slotUI);
        }

        slotUI.Init(item);
        return slotUI;
    }

    // Unit들의 프리팹들을 만듭니다.
    public Unit CreateUnit(UnitData unitData, Vector3 pos)
    {
        Unit unit = null;

        if (unitData is EnemyData)
        {
            unit = Instantiate(unitData.Model, pos, Quaternion.identity).GetComponent<Enemy>();
            unit.transform.rotation = Quaternion.Euler(0, 180, 0);
            
        }
        else if(unitData is PlayerData)
        {
            unit = Instantiate(playerPefab, pos, Quaternion.identity).GetComponent<Player>();
        }

        unit.Init(unitData);
        unit.Setup(unitData);

        return unit;
    }
}
