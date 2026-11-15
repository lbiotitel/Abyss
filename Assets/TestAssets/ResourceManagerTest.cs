using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResourceLoader;

/// <summary>
/// ResourceManager.cs�� ��巹���� �����͸� �����ϴ� ResourceLoader.cs�� ����ƽ���� ������ �ֽ��ϴ�.
/// ��巹���� ������ ������ �������� ��ɿ� ���ؼ��� ResourceLoader.cs�� �������ּ���.
/// </summary>
public class ResourceManagerTest : MonoBehaviour
{
    static private ResourceManager mResourceLoader;

    static public ResourceManager MResourceLoader
    {
        get { return mResourceLoader; }
        private set { mResourceLoader = value; }
    }

    private async void Awake()
    {
        mResourceLoader = this.GetComponent<ResourceManager>();
        Debug.Log($"{mResourceLoader} ���� �Ϸ�!");

        // ���ÿ� �ε��� �۾��� ����
        UniTask ingameTask = mResourceLoader.PreloadAssetsAsync("InGame");
        UniTask tycoonTask = mResourceLoader.PreloadAssetsAsync("Tycoon");

        // UniTask.WhenAll�� ���� �ε�
        // await UniTask.WhenAll(tycoonTask);
        await UniTask.WhenAll(ingameTask, tycoonTask);
    }
}
