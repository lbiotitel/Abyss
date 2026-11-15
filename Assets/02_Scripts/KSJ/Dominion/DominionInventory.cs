using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;

public class DominionInventory : MonoBehaviour
{
    [SerializeField] private InventorySystem invenSystem;
    [SerializeField] private string itemIDList;
    private List<ItemDataBase> ownedItemList;

    [Header("상점 UI")]
    [SerializeField] private DominionShopUI ohjShopUI;

    [Header("재료 UI")]
    [SerializeField] private TextMeshProUGUI textWood;
    [SerializeField] private TextMeshProUGUI textSteel;
    [SerializeField] private TextMeshProUGUI textWater;
    [SerializeField] private TextMeshProUGUI textEgg;
    [SerializeField] private TextMeshProUGUI textGold;

    [Header("CSV 저장")]
    [SerializeField] private string filepath;

    private void Awake()
    {
        filepath = Path.Combine(Application.dataPath, "02_Scripts/KSJ/DataLoad", "SavedInventory.csv");
    }

    private void Start()
    {
        invenSystem.Init();
        Invoke("Initilize", 1f);
    }

    private void OnEnable()
    {
        SceneTransition.canObjectToggle += ResetSavedInventory;
        ProduceCommodities.IsCommodityProduced += UpdateTextUI;
    }

    private void Initilize()
    {
        Managers.Game.Commodity.OnGoldChanged += UpdateTextUI;
    }

    private void UpdateTextUI()
    {
        textWood.text = "목재: " + Managers.Game.Commodity.commodities["10030_0002"].value.ToString() + "개";
        textSteel.text = "광석: " + Managers.Game.Commodity.commodities["10030_0001"].value.ToString() + "개";
        textWater.text = "식수: " + Managers.Game.Commodity.commodities["10030_0003"].value.ToString() + "개";
        textEgg.text = "계란: " + Managers.Game.Commodity.commodities["10030_0004"].value.ToString() + "개";
        textGold.text = Managers.Game.Commodity.gold.ToString() + "G";
    }

    public void ReadInventory()
    {
        ownedItemList = invenSystem.GetAllInventoryItemData();

        StringBuilder sb = new StringBuilder();

        // csv 헤더 작성
        sb.AppendLine("ItemCode");

        foreach (ItemDataBase item in ownedItemList)
        {
            sb.AppendLine(item.ItemID);
            itemIDList += item.ItemID + ",";
        }

        PlayerPrefs.SetString(KeyDefines.SaveKey.PLAYER_INVEN_DATA, itemIDList);

        File.WriteAllText(filepath, sb.ToString(), Encoding.UTF8);
        Debug.Log($"아이템 코드 csv 저장 완료!");

    }

    private void ResetSavedInventory()
    {
        itemIDList = "";

        // 던전에서 얻은 아이템은 영지로 가져오면 안된다
        // csv를 저장하고 던전 이동 시 영지의 인벤토리를 초기화해줘야 한다
        // (그래야 던전에서 영지로 돌아올 시 던전으로 가져간 아이템도 같이 초기화된다.)
        invenSystem.ClearItems();

        // 여기는 상점 풀 재생성
        ohjShopUI.ShopRefresh();
    }

    private void OnDisable()
    {
        SceneTransition.canObjectToggle -= ResetSavedInventory;
        Managers.Game.Commodity.OnGoldChanged -= UpdateTextUI;
    }

}
