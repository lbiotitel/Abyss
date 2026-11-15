using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading; // CancellationToken 사용을 위해 추가
using Cysharp.Threading.Tasks; // UniTask 사용을 위해 추가
using UnityEngine;

public class InGameManager
{
    public Player Player { get; private set; }
    public InventorySystem Inventory { get; private set; }
    public bool isGameStart { get; private set; } = false;
    public int CurrentGold { get; private set; }

    // game result stat
    public int KillCount { get; private set; }
    public float MoveDistance { get; private set; }
    public float PlayTime { get; private set; } 
    public float IncomingDamage { get; private set; }
    public float GivingDamage { get; private set; }

    // event
    public event Action<int> OnGoldChanged;

    // 비동기 루프 취소를 위한 소스
    private CancellationTokenSource playTimeCancelSource;

    public void GameStart()
    {
        if (isGameStart) return;

        isGameStart = true;

        CurrentGold = 0;
        KillCount = 0;
        MoveDistance = 0f;
        PlayTime = 0;
        IncomingDamage = 0;
        GivingDamage = 0;

        InitPlayer();
        InitInventory();

        // 게임 시작 시 타이머 작동 (Forget으로 비동기 루프 시작)
        StartPlayTimer().Forget();

        Managers.Event.NotifyEvent<OnInGameStart>(new OnInGameStart { });
    }


    //플레이어 생성및 초기화

    private void InitPlayer()
    {

        PlayerData playerData = Managers.Resource.GetAsset<PlayerData>(KeyDefines.ResourceKey.PLAYER_DATA);
        Unit unit = Managers.Object.CreateUnit(playerData, Vector3.zero);
        Player = unit.GetComponent<Player>();
    }





    //인벤토리 참조 및 초기화한 후 초기 아이템 설정

    private void InitInventory()
    {

        Inventory = GameObject.FindGameObjectWithTag(KeyDefines.Tag.PLAYER_INVENTORY)
            .GetComponent<InventorySystem>();

        Inventory.Init();

        string items = PlayerPrefs.GetString(KeyDefines.SaveKey.PLAYER_INVEN_DATA);
        string[] itemCodes = Utils.ParsingToItemCodes(items);

        itemCodes = itemCodes.Concat(KeyDefines.ResourceKey.START_ITEMS).ToArray();

        PlayerPrefs.DeleteKey(KeyDefines.SaveKey.PLAYER_INVEN_DATA);

        foreach (var itemCode in itemCodes)
        {
            if (itemCode == null || itemCode == string.Empty) continue;
            ItemDataBase itemData = Managers.Resource.GetAsset<ItemDataBase>(itemCode);
            InventoryItemSlotUI inventoryItem = Managers.Object.CreateInventoryItem(itemData);
            Inventory.AddToInventory(inventoryItem);
        }
    }




    // 1초마다 PlayTime을 늘려주는 UniTask 루프
    private async UniTaskVoid StartPlayTimer()
    {
        playTimeCancelSource?.Cancel();
        playTimeCancelSource = new CancellationTokenSource();
        var token = playTimeCancelSource.Token;

        try
        {
            while (isGameStart)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
                PlayTime += 1f;
            }
        }
        catch (OperationCanceledException)
        {
            Logger.Log("플레이 타임 타이머가 종료되었습니다.");
        }
    }



    public void GameOver()
    {
        if (!isGameStart) return;

        Logger.Log("여정이 끝났습니다.");
        isGameStart = false;

        playTimeCancelSource?.Cancel();
        playTimeCancelSource?.Dispose();
        playTimeCancelSource = null;

        Managers.Event.NotifyEvent<OnInGameOver>(new OnInGameOver { });
    }

    #region Gold
    public void SpendGold(int gold)
    {
        CurrentGold -= gold;
        OnGoldChanged?.Invoke(CurrentGold);
    }

    public void AddGold(int gold)
    {
        CurrentGold += gold;
        OnGoldChanged?.Invoke(CurrentGold);
    }

    public bool IsEnoughGold(int gold)
    {
        return CurrentGold >= gold;
    }
    #endregion


    public void AddMoveDistance() => MoveDistance++;
    public void AddGivingDamage(float damage) => GivingDamage += damage;    
    public void AddIncomingDamage(float damage) => IncomingDamage += damage;
    public void AddKillCount() => KillCount++;  
}