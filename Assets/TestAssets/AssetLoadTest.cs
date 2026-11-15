using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetLoadTest : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite sprite;
    [SerializeField] private List<ScriptableObject> testGetAssets;
    [SerializeField] private List<EquipmentData> testtest;
    [SerializeField] private List<ItemDataBase> itemtest;
    [SerializeField] private PlayerData playerdata;
    [SerializeField] private GameObject playerprefab;
    [SerializeField] private List<GameObject> testGetPrefabs;
    [SerializeField] private ScriptableObject weapon;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //Invoke("LoadTest", 1f);
        Managers.Resource.IsDataFullyLoaded += LoadMultiple;
        //Invoke("LoadMultiple", 2f);

        Invoke("LoadPrefab", 2f);
    }

    private void LoadTest()
    {
        if (Managers.Resource == null) Debug.Log("리소스 로더 할당 안됐다...");
        //sprite = ResourceManager.MResourceLoader.GetAsset<Sprite>("test_sprite1");
        sprite = Managers.Resource.GetAsset<Sprite>("test_sprite1");
        spriteRenderer.sprite = sprite;
    }

    private void LoadMultiple()
    {
            testGetAssets = Managers.Resource.GetAssets<ScriptableObject>();
            testtest = Managers.Resource.GetAssets<EquipmentData>();
            itemtest = Managers.Resource.GetAssets<ItemDataBase>();
        testGetPrefabs = Managers.Resource.GetAssets<GameObject>();

            // GameObject clone = Instantiate(ResourceManager.MResourceLoader.GetAsset<GameObject>("TestPrefab1 1"));
    }

    private void LoadPrefab()
    {
        playerdata = Managers.Resource.GetAsset<PlayerData>("Player Prefab");
        playerprefab = Managers.Resource.GetAsset<GameObject>("Player Prefab");
        weapon = Managers.Resource.GetAsset<ScriptableObject>("10010_WE_001");

    }
}
