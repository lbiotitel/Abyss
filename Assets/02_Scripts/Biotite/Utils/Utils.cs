using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Utils : MonoBehaviour
{


    public static async void Delay(float time, Action action)
    { 
        await UniTask.WaitForSeconds(time);
        action?.Invoke();
    }


    //PlayerPrefs에 저장된 키값을 파싱하여 돌려줍니다.    
    public static string[] ParsingToItemCodes(string str)
    {
        string[] items = str.Split(",");
        return items;
    }

    public static string ItemTypeToKor(EItemType itemType)
    {
        switch (itemType)
        {
            case EItemType.Equipment:
            return "장비";
            case EItemType.Consumable:
            return "소모품";
            case EItemType.Resource:
            return "재료";
        }

        return "";
    }

    public static string itemRarityToKor(ERarity rarity)
    {
        switch(rarity)
        {
            case ERarity.Normal:
            return "일반";
            case ERarity.Rare:
            return "레어";
            case ERarity.Unique:
            return "유니크";
            case ERarity.Legend:
            return "전설";
        }

        return "";
    }

    public static Color GetRarityColor(ERarity rarity)
    {
        switch(rarity)
        {
            case ERarity.Normal:
            return Color.white;
            case ERarity.Rare:
            return new Color(0.2f, 0.6f, 1.0f);
            case ERarity.Unique:
            return Color.magenta;
            case ERarity.Legend:
            return Color.yellow;
        }

        return Color.white;
    }



    //재귀적으로 자식을 다 찾아 특정한 오브젝트의 위치를 찾습니다.
    public static Transform Find(string name, Transform transform)
    {
        if (name == transform.name)
        {
            return transform;
        }

        //자식을 재귀로 다 찾아봄
        Transform find = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            find = Find(name, child);
            if (find != null) return find;
        }

        return null;
    }

}
