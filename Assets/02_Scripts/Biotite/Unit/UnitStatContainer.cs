using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;



//유닛의 스탯을 관리하는 컨테이너 입니다.
public class UnitStatContainer
{

    private Dictionary<EStatType, StatBase> statDict;

    public event Action<EStatType, float, float> OnStatValueChanged;


    //스탯을 초기화합니다.
    public void Init(UnitData data)
    {
        statDict = new Dictionary<EStatType, StatBase>();
        InitStats(data);
    }


    //플레이어 데이터에 등록된 되로 스탯을 초기화합니다
    private void InitStats(UnitData unitData)
    {
        List<SStatContainer> container = unitData.StatContainers;
        foreach (var statData in container)
        {
            if (IsResourceStat(statData.statType))
            {
                if (EStatType.CurrentHP == statData.statType)
                    statDict[statData.statType] = new ResourceStat(statData.value, statData.statType, statDict[EStatType.MaxHp]);
                else
                    statDict[statData.statType] = new ResourceStat(statData.value, statData.statType, statDict[EStatType.MaxMP]);
            }
            else
            {
                statDict[statData.statType] = new ModifireStat(statData.value, statData.statType);
            }
        }

        BindEvents();
    }


    //자원 스탯인지 판단 합니다
    private bool IsResourceStat(EStatType type)
    {
        bool isHp = type == EStatType.CurrentHP;

        bool isMp = type == EStatType.CurrentMP;

        return isHp || isMp;
    }


    //스탯의 변화를 감지하는 이벤트를 바인딩 하고 변화시 외부에 알립니다.
    private void BindEvents()
    {
        foreach (var dict in statDict)
        {
            dict.Value.OnStatValueChanged += HandleEvent;
        }
    }

    //스탯의변화를 감지하고 외부에 알리는 메서드 입니다.
    private void HandleEvent(EStatType statType, float prevValue, float newValue)
        => OnStatValueChanged?.Invoke(statType, prevValue, newValue);


    //스탯 변화를 추가합니다.
    public void AddModifire(EStatType statType, StatModifire statModifire)
    {
        if (statDict.TryGetValue(statType, out StatBase stat))
        {
            stat.AddModifire(statModifire);
        }
    }


    //스탯 변화를 제거합니다.
    public void RemoveModifire(EStatType statType, StatModifire statModifire)
    {
        if (statDict.TryGetValue(statType, out StatBase stat))
        {
            stat.RemoveModifire(statModifire);
        }
    }


    //특정 스탯의 최종 값을 return 합니다.
    public float GetStatValue(EStatType statType)
    {
        if (statDict.TryGetValue(statType, out StatBase stat))
        {
            return stat.GetValue();
        }

        return 0;
    }
}
