using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Threading;
using System;

public class ProduceCommodities : MonoBehaviour
{

    private PlaceInformation placeInfo;
    [SerializeField] private int count = 0;
    private CancellationTokenSource source;
    static public Action IsCommodityProduced;

    private void Awake()
    {
        placeInfo = gameObject.GetComponent<PlaceInformation>();
    }

    public void StartProduce(PlaceInformation placeInfo, bool isProducing)
    {
       if (isProducing == false) return;

        // 중복 생산을 방지하기 위해 기존 작업을 안전하게 취소
        StopProduce();

        Debug.Log($"{placeInfo.PlaceName} 생산 시작한다!");

        source = new CancellationTokenSource();

        var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(source.Token, this.GetCancellationTokenOnDestroy());

        ProduceTimer(linkedToken.Token).Forget();
    }

    public void StopProduce()
    {
        if(source != null)
        {
            source.Cancel();
            source.Dispose();
            source = null;
        }
    }

    private async UniTaskVoid ProduceTimer(CancellationToken cancellationToken)
    {
        // 토큰에 취소 요청이 들어오기 전까지 반복
        while(!cancellationToken.IsCancellationRequested)
        {
            try
            {
                // 유니테스크는 밀리초 단위이므로 밀리초에 맞춰서 숫자 계산
                int delayMs = placeInfo.CommodityProdTime * 1000;

                await UniTask.Delay(delayMs, cancellationToken: cancellationToken);
                
                // 시간이 지났으므로 아이템 획득
                count++;
                Managers.Game.Commodity.GetCommodity(placeInfo.CommodityID, placeInfo.CommodityAmount);
                IsCommodityProduced?.Invoke();
                /*
                ItemDataBase itemData = Managers.Resource.GetAsset<ItemDataBase>(itemCode);
                InventoryItemSlotUI inventoryItem = Managers.Object.CreateInventoryItem(itemData);
                Inventory.AddToInventory(inventoryItem);
                */
            }

            catch (OperationCanceledException)
            {
                // UniTask.Delay 도중에 Cancel() 호출 시 예외 catch 예외가 발생하면서
                // while 루프를 빠져나가며 안전하게 종료한다.
                break;
            }
        }
    }

    // 오브젝트 파괴 시 메모리 누수 방지
    private void OnDestroy()
    {
        StopProduce();
    }
}
