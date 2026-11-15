using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommodityManager
{
    public Dictionary<string, Commodity> commodities = new Dictionary<string, Commodity>();
    public int gold = 0;

    public Action OnGoldChanged;
    public Action IsInitialized;

    public void Init()
    {
        commodities.Add("10030_0002", new Commodity("Wood", 0));
        commodities.Add("10030_0001", new Commodity("Steel", 0));
        commodities.Add("10030_0003", new Commodity("Water", 0));
        commodities.Add("10030_0004", new Commodity("Egg", 0));

        gold = 0;

        Debug.Log("재화 매니저 소환!");
        IsInitialized?.Invoke();
    }

    public bool GetCommodity(string id, int amount)
    {
        // 먼저 해당 키값의 재화가 있는지 확인
        if (!commodities.ContainsKey(id))
        {
            Debug.LogError($"{id} 재화는 존재하지 않는다! 꿈에서 깨!");
            return false;
        }

        Commodity targetCommodity = commodities[id];

        targetCommodity.value += amount;

        // UI 갱신
        ////// DEBUG //////
        OnGoldChanged?.Invoke();

        return true;
    }

    public bool SpendCommodity(string id, int amount)
    {
        // 먼저 해당 키값의 재화가 있는지 확인
        if (!commodities.ContainsKey(id))
        {
            Debug.LogError($"{id} 재화는 존재하지 않는다! 꿈에서 깨!");
            return false;
        }

        Commodity targetCommodity = commodities[id];

        // 재화가 충분히 있는지 확인
        if (targetCommodity.value < amount)
        {
            Debug.LogWarning($"{targetCommodity} 재화는 보유량이 부족하다!");
            return false;
        }

        targetCommodity.value -= amount;

        // UI 갱신
        OnGoldChanged?.Invoke();

        return true;

    }

    public void AddGold(int amount)
    {
        if (amount <= 0) return;
        gold += amount;

        // UI 갱신
        OnGoldChanged?.Invoke();
    }

    public void SpendGold(int amount)
    {
        if (amount <= 0) return;

        if(gold < amount)
        {
            Debug.LogError("소모량이 현재 골드 보유량보다 많아!");
        }

        else
        {
            gold -= amount;
        }

        // UI 갱신
        OnGoldChanged?.Invoke();
    }

    public int CheckGoldAmount()
    {
        return gold;
    }

    public class Commodity
    {
        public string itemID;
        public int value = 0;

        public Commodity(string itemID, int value)
        {
            this.itemID = itemID;
            this.value = value;
        }
    }
}
