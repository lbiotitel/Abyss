using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceInformation : MonoBehaviour
{
    [Header("기본 정보")]
    [SerializeField] private string placeName;
    [SerializeField] private bool isProducing = false;

    [Header("생산품 정보")]
    [SerializeField] private string commodityID;
    [SerializeField] private int commodityAmount = 1;        // 대기 시간 달성 시 지급되는 생산품의 수량
    [SerializeField] private int commodityProdTime = 10;     // 생산품을 commodityAmount만큼 얻기 위해 필요한 대기 시간

    [Header("건물 정보")]
    [SerializeField] private int placeLevel = 1;

    public string PlaceName
    {
        get { return placeName; }
        private set { placeName = value; }
    }

    public bool IsProducing
    {
        get { return isProducing; }
        set { isProducing = value; }
    }

    public string CommodityID
    {
        get { return commodityID; }
        private set { commodityID = value; }
    }

    public int CommodityAmount
    {
        get { return commodityAmount; }
        private set { commodityAmount = value; }
    }

    public int CommodityProdTime
    {
        get { return commodityProdTime; }
        private set { commodityProdTime = value; }
    }

    public int PlaceLevel
    {
        get { return placeLevel; }
        private set { placeLevel = value; }
    }

}

