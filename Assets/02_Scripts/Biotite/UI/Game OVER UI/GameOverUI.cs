using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TMP_Text killCountText;
    [SerializeField] private TMP_Text playTimeText;
    [SerializeField] private TMP_Text moveDistanceText;
    [SerializeField] private TMP_Text goldText;

    [SerializeField] private Transform gridLayout;

    [SerializeField] private Button returnButton;
    private Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
        Init();
    }

    private void OnDestroy()
    {
        Managers.Event.RemoveEvent<OnInGameOver>(HandleOnInGameOver);
    }

    //게임 종료 ui를 초기화합니다.
    private void Init()
    {
        Managers.Event.AddEvent<OnInGameOver>(HandleOnInGameOver);
        returnButton.onClick.AddListener(ReturnToDominion);
    }


    //영지로 돌아갑니다. 플레이어가 사망하지않으면 골드를 들고 영지로 갑니다.
    private void ReturnToDominion()
    {
        Player player = Managers.Game.InGame.Player;
        if (!player.IsDead)
        {
            int currentGold = Managers.Game.InGame.CurrentGold;
            Managers.Game.Commodity.AddGold(currentGold);
        }
        Hide();
    }

    private void HandleOnInGameOver(OnInGameOver e) => Show();


    public void Hide() => gameObject.SetActive(false);
    private void Show()
    {
        animator.SetTrigger("Show");

        killCountText.text = $"{Managers.Game.InGame.KillCount}";
        float playTime = Managers.Game.InGame.PlayTime;
        playTimeText.text = $"{(int)((int)playTime / 60)}m:{playTime % 60}s";
        moveDistanceText.text = $"{Managers.Game.InGame.MoveDistance / 100f}m";
        goldText.text = $"{Managers.Game.InGame.CurrentGold}G";

        CreateInventoryLootingItem();
    }


    private void CreateInventoryLootingItem()
    {
        List<ItemDataBase> itemDatas = Managers.Game.InGame.Inventory.GetAllInventoryItemData();
        foreach (var itemData in itemDatas)
        {
            GameObject image = new GameObject("Looting Item");
            Image im = image.AddComponent<Image>();
            im.sprite = itemData.ItemIcon;
            im.preserveAspect = true;
            im.transform.SetParent(gridLayout);
        }
    }
}
