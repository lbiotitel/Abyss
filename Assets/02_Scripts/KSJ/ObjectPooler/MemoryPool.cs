using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MemoryPool<T> where T : UnityEngine.Object
{
    private class PoolItem
    {
        public bool isActive;               // 활성화 정보
        public T targetObject;              // 풀링 대상 보관
        public GameObject gameObject;       // 실질적인 SetActive를 위한 게임오브젝트 참조
    }

    private int increaseCount = 5;          // 오브젝트가 부족할 때 Instantiate()로 추가 생성되는 오브젝트 개수
    private int maxCount;
    private int activeCount;

    private T prefab;                       // 생성할 원본 프리팹
    private Transform parentTransform;      // 하이어라키 정리를 위한 부모 트랜스폼
    private List<PoolItem> poolItemList;    // 관리되는 모든 오브젝트를 저장하는 리스트

    // 생성자에서 프리팹과 생성될 위치를 받아옴
    public MemoryPool(T prefab, int initialCount = 5, Transform parent = null)
    {
        this.prefab = prefab;
        this.increaseCount = initialCount;
        this.parentTransform = parent;

        maxCount = 0;
        activeCount = 0;

        poolItemList = new List<PoolItem>();

        InstantiateObjects();
    }

    /// <summary>
    /// increaseCount 단위로 오브젝트 생성
    /// </summary>
    public void InstantiateObjects()
    {
        maxCount += increaseCount;

        for(int i = 0; i < increaseCount; ++i)
        {
            PoolItem poolItem = new PoolItem();

            poolItem.isActive = false;

            // 프리팹 생성 및 부모 설정
            T instance = GameObject.Instantiate(prefab, parentTransform);
            poolItem.targetObject = instance;

            if(instance is GameObject go)
            {
                poolItem.gameObject = go;
            }

            else if(instance is Component comp)
            {
                poolItem.gameObject = comp.gameObject;
            }

            poolItem.gameObject.SetActive(false);
            poolItemList.Add(poolItem);
        }
    }

    /// <summary>
    /// 활성 상태에 상관없이 현재 관리중인 모든 오브젝트를 삭제
    /// </summary>
    public void DestroyObjects()
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;

        for (int i = 0; i < count; ++i)
        {
            // 하이어라키에서 이미 삭제된 오브젝트가 있을 수 있으므로 null 체크하기
            if(poolItemList[i].gameObject != null)
            {
                GameObject.Destroy(poolItemList[i].gameObject);
            }
        }

        poolItemList.Clear();

        maxCount = 0;
        activeCount = 0;
    }

    /// <summary>
    /// poolItemList에 저장된 오브젝트를 활성화하여 사용
    /// </summary>
    /// <returns></returns>
    public T ActivatePoolItem()
    {
        if (poolItemList == null) return null;

        // 모든 오브젝트가 활성화 상태이면 새로운 오브젝트 생성
        if (maxCount == activeCount) InstantiateObjects();

        int count = poolItemList.Count;

        for(int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if(poolItem.isActive == false)
            {
                activeCount++;

                poolItem.isActive = true;
                poolItem.gameObject.SetActive(true);

                return poolItem.targetObject;
            }
        }

        return null;
    }

    /// <summary>
    /// 현재 사용이 완료된 오브젝트를 비활성화 상태로 전환
    /// </summary>
    /// <param name="removeObject"></param>
    public void DeactivatePoolItem(T removeObject)
    {
        if (poolItemList == null || removeObject == null) return;

        int count = poolItemList.Count;

        for(int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if(poolItem.targetObject == removeObject)
            {
                activeCount--;

                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);

                return;
            }
        }
    }

    /// <summary>
    /// 게임에 사용중인 모든 오브젝트를 비활성화 상태로 설정
    /// </summary>
    public void DeactivateAllPoolItems()
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;

        for(int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.targetObject != null && poolItem.gameObject != null && poolItem.isActive == true)
            {
                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);
            }
        }

        activeCount = 0;
    }
}
