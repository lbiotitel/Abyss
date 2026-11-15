using System.Collections.Generic;
using UnityEngine;


//������Ʈ Ǯ���� ����մϴ�.
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

    //������Ʈ�� �����ɴϴ�. 
    public T GetObject<T>(GameObject goPrefab, Vector3 position, Quaternion rotation) where T : Component
    {
        GameObject res = GetObject(goPrefab, position, rotation);
        if (res.TryGetComponent<T>(out T component))
        {
            return component;
        }

        Debug.LogError("�ش�Ǵ� ������Ʈ�� �����ϴ�.");
        return null;
    }


    //������Ʈ�� �����ɴϴ�. ���ٸ� ���θ���� Ǯ�� �մϴ�.
    public GameObject GetObject(GameObject goPrefab, Vector3 position, Quaternion rotation)
    {
        if (!goPrefab.TryGetComponent(out IPoolingObject _))
        {
            Debug.LogError($"Ǯ���� ������ ���� ������ �Դϴ�. Ǯ���� �ҷ��� IPoolingObject�� ���̼���");
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

    //�ڽ��� Clone�� ���� �������� ã���ϴ�.
    private GameObject GetObjectOriginPrefab(GameObject go)
    {
        if (go.TryGetComponent(out IPoolingObject poolingObject))
        {
            return poolingObject.OriginPrefab;
        }

        return null;
    }


    //Ǯ���� ������Ʈ���� �ٽ� �����մϴ�.
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
            Debug.LogError("Ǯ�� ���� ���� ������Ʈ�� Return �Ǿ����ϴ�.");
        }
    }


    //Ǯ�� �ִ� ������Ʈ�� ���忡 �ִ� Ǯ�� �ϴ� ������Ʈ�� ��� �ı��մϴ�.
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
