using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;



//������ ���� �����Դϴ�.
public class DominionShopUI : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    [SerializeField] private ShopSlotUI shopNodeUIPrefab;
    [SerializeField] private Transform gridLayoutTransform;
    [SerializeField] private int shopSlotCount;

    [SerializeField] private int equipmentSlotCount;

    List<ShopSlotUI> shopNodeUIs;
    List<EquipmentData> equipmentDatas;
    List<ConsumableData> consumptionDatas;


    private void Start()
    {
        Init();
        ShopRefresh();
        Show();
    }

    //������ �ʿ��� �����͸� ĳ���ϰ� �ʱ�ȭ�մϴ�.
    public void Init()
    {
        var equipmentList = Managers.Resource.GetAssets<EquipmentData>();
        var filterList = equipmentList.Where((x) => x.Rarity == ERarity.Normal && x.InShop == true);
        equipmentDatas = filterList.ToList<EquipmentData>();
        var consumptionList = Managers.Resource.GetAssets<ConsumableData>();
        var cFilterList = consumptionList.Where((x) => x.InShop == true);
        consumptionDatas = cFilterList.ToList<ConsumableData>();
        exitButton.onClick.AddListener(() => Hide());
        InitShopNodeUIs();
    }



    //������ ������ ������ Ǯ�� �� �Ӵϴ�.
    private void InitShopNodeUIs()
    {
        shopNodeUIs = new List<ShopSlotUI>();
        for (int i = 0; i < shopSlotCount; i++)
        {
            ShopSlotUI shopNodeUI = Instantiate(shopNodeUIPrefab, gridLayoutTransform);
            shopNodeUI.gameObject.SetActive(true);
            shopNodeUI.OnClicked += HandleClickedEvent;
            shopNodeUIs.Add(shopNodeUI);
        }
    }

    private void OnDestroy()
    {
        foreach (var nodeUI in shopNodeUIs)
        {
            nodeUI.OnClicked -= HandleClickedEvent;
        }
    }


    //Ư�� ������ Ŭ���ҽ� ���� ����� shop���� Ŭ���� �����͸� �޾� ó�� �մϴ�.
    private void HandleClickedEvent(ShopSlotUI slot, ItemDataBase itemData)
    {

        Logger.Log($"{itemData.ItemName}�� �����ҷ��� �մϴ�.");

        int currentGold = Managers.Game.Commodity.gold;
        if (currentGold >= itemData.ItemPrice)
        {
            Managers.Game.Commodity.SpendGold(itemData.ItemPrice);
            slot.Sell();
            InventorySystem inventorySystem = GameObject.FindAnyObjectByType<InventorySystem>();
            InventoryItemSlotUI inventoryItem = Managers.Object.CreateInventoryItem(itemData);
            inventorySystem.AddToInventory(inventoryItem);
        }
    }


    //�־��� �����Ϳ��� ������ŭ �������� �̽��ϴ�.
    private List<T> GenerateRandomEquipmentData<T>(int count) where T : ItemDataBase
    {
        List<T> result = new();

        List<T> sourceDatas = null;

        if (typeof(T) == typeof(EquipmentData))
        {
            sourceDatas = equipmentDatas as List<T>;
        }
        else if (typeof(T) == typeof(ConsumableData))
        {
            sourceDatas = consumptionDatas as List<T>;
        }

        if (sourceDatas == null || sourceDatas.Count == 0)
            return result;

        for (int i = 0; i < count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, sourceDatas.Count);
            result.Add(sourceDatas[randomIndex]);
        }

        return result;
    }



    //�������� ���� �����͸� ������ ���Կ��� �����͸� �ѱ�ϴ�.
    private void ShopLogic()
    {
        List<EquipmentData> randomEquipmentDatas =
            GenerateRandomEquipmentData<EquipmentData>(equipmentSlotCount);
        List<ConsumableData> randomConsumableDatas =
            GenerateRandomEquipmentData<ConsumableData>(shopSlotCount - equipmentSlotCount);

        int index = 0;
        for (int i = 0; i < randomEquipmentDatas.Count; i++)
        {
            ShopSlotUI slot = shopNodeUIs[index++];
            slot.Init(randomEquipmentDatas[i]);
            slot.Show();
        }

        for (int i = 0; i < randomConsumableDatas.Count; i++)
        {
            ShopSlotUI slot = shopNodeUIs[index++];
            slot.Init(randomConsumableDatas[i]);
            slot.Show();
        }
    }

    public void ShopRefresh() => ShopLogic(); 
    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);

}
