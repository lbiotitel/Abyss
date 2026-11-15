using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingTest : MonoBehaviour
{
    [SerializeField] private GameObject obj;

    // 메모리풀 선언
    private MemoryPool<GameObject> objPool;

    private void Awake()
    {
        // 오브젝트 or 컴포넌트를 메모리풀에 등록
        objPool = new MemoryPool<GameObject>(obj, 10, this.transform);

        StartCoroutine("InstantiateObject");
    }

    private IEnumerator InstantiateObject()
    {
        // 풀에 등록된 오브젝트 생성
        GameObject obj = objPool.ActivatePoolItem();

        if(obj != null)
        {
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
        }

        yield return new WaitForSeconds(5f);

        ReturnObject(obj);

    }

    private void ReturnObject(GameObject obj)
    {
        // 풀에 등록된 오브젝트 내리기
        objPool.DeactivatePoolItem(obj);
    }
}
