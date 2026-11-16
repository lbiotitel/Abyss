using System.Collections.Generic;
using UnityEngine;


//오브젝트 풀링을 담당합니다.
public class PoolingSystem : MonoBehaviour
{
    private Dictionary<GameObject, Queue<GameObject>> poolDict;

    public void Init()
    {
        poolDict = new Dictionary<GameObject, Queue<GameObject>>();
        Managers.Event.AddEvent<OnInGameOver>(HandleOnInGameOver);
    }

    private void OnDisable()
    {
        Managers.Event.RemoveEvent<OnInGameOver>(HandleOnInGameOver);
    }


    //게임 오버시 풀링된 오브젝트 Clear
    private void HandleOnInGameOver(OnInGameOver e)
    {
        ClearPools();
    }

    //풀에서 오브젝트를 가져와 원하는 컴포넌트로 반환합니다.
    public T GetObject<T>(GameObject goPrefab, Vector3 position, Quaternion rotation) where T : Component
    {
        GameObject res = GetObject(goPrefab, position, rotation);
        if (res.TryGetComponent<T>(out T component))
        {
            return component;
        }

        Debug.LogError("해당되는 컴포넌트가 없습니다.");
        return null;
    }


    //풀에서 오브젝트를 가져옵니다. 없다면 새로 만들어서 풀링합니다.
    public GameObject GetObject(GameObject goPrefab, Vector3 position, Quaternion rotation)
    {
        if (!goPrefab.TryGetComponent(out IPoolingObject _))
        {
            Debug.LogError($"풀링할 수 없는 프리팹입니다. 풀링하려면 IPoolingObject를 붙이세요.");
            return null;
        }

        GameObject res = null;

        if (!poolDict.ContainsKey(goPrefab))
        {
            Queue<GameObject> newPool = new Queue<GameObject>();
            poolDict.Add(goPrefab, newPool);
        }

        Queue<GameObject> pool = poolDict[goPrefab];

        if (pool.Count <= 0)
        {
            GameObject poolingObject = Instantiate(goPrefab, position, rotation);
            poolingObject.GetComponent<IPoolingObject>().OriginPrefab = goPrefab;
            res = poolingObject;
        }
        else
        {
            res = poolDict[goPrefab].Dequeue();
        }

        res.transform.SetParent(null);
        res.transform.position = position;
        res.transform.rotation = rotation;
        res.gameObject.SetActive(true);

        return res;
    }

    //자신을 Clone한 원본 프리팹을 찾습니다.
    private GameObject GetObjectOriginPrefab(GameObject go)
    {
        if (go.TryGetComponent(out IPoolingObject poolingObject))
        {
            return poolingObject.OriginPrefab;
        }

        return null;
    }


    //사용이 끝난 오브젝트를 풀에 반환합니다.
    public void ReturnObject(GameObject returnObject)
    {
        GameObject prefabKey = GetObjectOriginPrefab(returnObject);

        if (poolDict.ContainsKey(prefabKey))
        {
            returnObject.gameObject.SetActive(false);
            returnObject.transform.SetParent(transform);

            returnObject.transform.position = transform.position;
            returnObject.transform.rotation = Quaternion.identity;

            poolDict[prefabKey].Enqueue(returnObject);
        }
        else
        {
            Debug.LogError("풀에 없는 오브젝트가 Return 되었습니다.");
        }
    }


    //풀에 있는 오브젝트와 월드에 있는 풀링 오브젝트를 모두 파괴합니다.
    private void ClearPools()
    {
        var worldObjects = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        foreach (var mono in worldObjects)
        {
            if (mono is IPoolingObject)
            {
                Destroy(mono.gameObject);
            }
        }

        foreach (var queue in poolDict.Values)
        {
            foreach (var go in queue)
            {
                if (go != null)
                    Destroy(go);
            }
            queue.Clear();
        }
        poolDict.Clear();
    }

    public void Clear()
    {
        ClearPools();
    }
}
