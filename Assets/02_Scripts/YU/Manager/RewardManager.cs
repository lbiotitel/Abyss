using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RewardManager
{
    // 현재 스테이지의 아이템들
    private List<string> _currentMapItemPool = new List<string>();

    // 초기화
    public void Init()
    {
        // 장착아이템(무기, 방어구)과 소모품 아이템들만 가져오기
        List<EquipmentData> equips = Managers.Resource.GetAssets<EquipmentData>();
        List<ConsumableData> consumables = Managers.Resource.GetAssets<ConsumableData>();

        _currentMapItemPool.Clear();

        // 상점에서 살 수 있는 아이템들과 ID가 같은 아이템들만 가져와서 리스트에 넣기
        _currentMapItemPool.AddRange(equips.Where(x => x.InShop).Select(x => x.ItemID));
        _currentMapItemPool.AddRange(consumables.Where(x => x.InShop).Select(x => x.ItemID));
    }

    // 내가 원하는 개수만큼 랜덤으로 아이템들을 뽑기
    public List<string> GetRandomRewards(int count)
    {
        if (_currentMapItemPool.Count == 0) return new List<string>();

        List<string> tempPool = new List<string>(_currentMapItemPool); // 복사본
        List<string> result = new List<string>();                      // 결과

        int actualCount = Mathf.Min(count, tempPool.Count);

        for (int i = 0; i < actualCount; i++)
        {
            int index = Random.Range(0, tempPool.Count);
            result.Add(tempPool[index]);
            tempPool.RemoveAt(index); // 이번 보상 목록 내에서의 중복 방지
        }

        return result;
    }
}
