using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


//보물 상자이 로직 정의 입니다.
public class TreasureChest : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private float interactDistance = 2.0f;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private InventorySystem inventorySystem;
    [SerializeField] private GameObject blockingObject;
    private Animator animator;
    private bool isOpen = false;
    private Player player;

    //test
    [SerializeField] private List<string> itemCodes;


    private void Start()
    {
        isOpen = false;
        animator = GetComponent<Animator>();
        inventorySystem.Init();
        player = Managers.Game.InGame.Player;
        HideUI();
    }


    //보물상자의 보상을 만듭니다.
    private void CreateReward()
    {
        List<string> itemCodes = Managers.Reward.GetRandomRewards(2);

        foreach (var itemCode in itemCodes)
        {
            ItemDataBase itemData = Managers.Resource.GetAsset<ItemDataBase>(itemCode);
            InventoryItemSlotUI inventoryItem = Managers.Object.CreateInventoryItem(itemData);
            inventorySystem.AddToInventory(inventoryItem);
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (isOpen || !CheckDistance()) return;
        isOpen = true;
        animator.SetTrigger("Open");
        blockingObject.SetActive(false);
        Managers.Game.InGame.Player.ChangeState(UnitState.Stop);
    }


    //열리는 애니메이션이 끝날 때 보물상자 ui를 표시합니다.
    public void AnimationFinishedEvent()
    {
        Logger.Log("보물 상자가 열렸습니다.");
        CreateReward();
        ShowUI();
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isOpen || !CheckDistance()) return;
        animator.SetTrigger("Open Slightly");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isOpen || !CheckDistance()) return;
        animator.SetTrigger("Close Slightly");
    }

    private bool CheckDistance()
    {
        float dis = Vector3.Distance(transform.position, player.transform.position);
        return dis <= interactDistance;
    }

    public void ShowUI() => inventoryUI.SetActive(true);
    public void HideUI()
    {
        inventoryUI.SetActive(false);
        Managers.Game.InGame.Player.ChangeState(UnitState.Idle);
    }


}
