using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

class BattleReword : MonoBehaviour
{
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private InventorySystem inventorySystem;

    private void Start()
    {
        Managers.Event.AddEvent<OnReward>(Open);

        inventorySystem.Init();
        inventoryUI.SetActive(false);
    }

    private void OnDestroy()
    {
        Managers.Event.RemoveEvent<OnReward>(Open); 
    }

    private void Open(OnReward e)
    {
        ShowUI();
        CreateReward(e.rewards);
    }


    //배틀의 보상을 만듭니다.
    private void CreateReward(List<string> itemcodes)
    {
        Managers.Game.InGame.AddGold(100);

        foreach (var code in itemcodes)
        {
            ItemDataBase itemData = Managers.Resource.GetAsset<ItemDataBase>(code);
            InventoryItemSlotUI inventoryItem = Managers.Object.CreateInventoryItem(itemData);
            inventorySystem.AddToInventory(inventoryItem);
        }
    }

    public void ShowUI() => inventoryUI.SetActive(true);
    public void HideUI()
    {
        inventoryUI.SetActive(false);
            Managers.Game.InGame.Player.ChangeState(UnitState.Idle);
    }
}
