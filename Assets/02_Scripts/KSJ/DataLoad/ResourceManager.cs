using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;
using System;

using Object = UnityEngine.Object;
using System.Security.Cryptography;

namespace ResourceLoader
{
    public class ResourceManager : MonoBehaviour
    {
        public Action IsDataFullyLoaded;

        private readonly Dictionary<string, AssetItem> loadedAssets = new Dictionary<string, AssetItem>();

        private readonly List<ScriptableObject> clonedSO = new List<ScriptableObject>();


        public async UniTask LoadResources()
        {
             // 동시에 로드할 작업을 생성
            UniTask ingameTask = PreloadAssetsAsync("InGame");
            UniTask tycoonTask = PreloadAssetsAsync("Tycoon");

            // UniTask.WhenAll로 병렬 로드
            // await UniTask.WhenAll(tycoonTask);
            await UniTask.WhenAll(ingameTask, tycoonTask);

            Debug.Log("데이터 모두 로드됨! IsDataFullyLoaded 이벤트 호출!");
            IsDataFullyLoaded?.Invoke();
        }

        // 라벨 단위의 에셋을 메모리에 등록하기
        public async UniTask PreloadAssetsAsync(string label)
        {
            // 콜백을 사용해 바로 loadedAssets에 추가
            AsyncOperationHandle<IList<Object>> handle = Addressables.LoadAssetsAsync<Object>(label, asset =>
            {
                if (asset == null) return;
                if (loadedAssets.ContainsKey(asset.name)) return;

                // Sprite는 Texture2D로 인식되므로 예외 처리
                if (asset is Texture2D texture)
                {
                    Rect rect = new Rect(0, 0, texture.width, texture.height);
                    Vector2 pivot = new Vector2(0.5f, 0.5f);

                    Sprite sprite = Sprite.Create(texture, rect, pivot);

                    // 인게임은 sprite, 어드레서블 해제 시에는 원본 텍스처를 저장
                    loadedAssets[asset.name] = new AssetItem { targetAsset = sprite, originalAsset = texture };
                }

                // 기타 파일
                else if (asset != null && !loadedAssets.ContainsKey(asset.name))
                {
                    // 일반 에셋은 인게임과 해제용이 동일함
                    loadedAssets[asset.name] = new AssetItem { targetAsset = asset, originalAsset = asset };
                }

                // Debug.Log($"로드된 에셋: {asset.name}, 에셋 타입: {asset.GetType()}");
            });

            IList<Object> result = await handle.ToUniTask();

            //Debug.Log(result.Count);

            if (result == null || result.Count == 0)
            {
                Debug.LogError($"{label} 라벨로 등록된 어드레서블 데이터가 있는지 확인하쇼...");
            }
            /*
            foreach(string s in loadedAssets.Keys)
            {
                Debug.Log($"{s} 로드함!");
            }
            */
        }

        /// <summary>
        /// 에셋 단일 반환
        /// 예시: Managers.Resource.GetAsset<타입>("어드레서블 주소");
        /// </summary>
        /// <typeparam name="T">가져오려는 데이터 에셋의 타입을 입력해주세요.</typeparam>
        /// <param name="key">어드레서블 주소 적어주세요.</param>
        /// <returns></returns>
        public T GetAsset<T>(string key) where T : Object
        {
            if (loadedAssets.TryGetValue(key, out var asset))
            {
                T original = asset.targetAsset as T;

                if (original != null)
                {
                    if (original is ScriptableObject)
                    {
                        T clone = Instantiate(original);
                        clonedSO.Add(clone as ScriptableObject);
                        return clone;
                    }
                }

                return original;
            }

            // 로드가 안됐다면
            Debug.LogError($"{key} 에셋을 찾을 수 없으므로 로드하지 못했다...");
            return null;
        }

        /// <summary>
        /// 복수 에셋 반환
        /// 예시: Managers.Resource.GetAssets<타입>();
        /// </summary>
        /// <typeparam name="T">한꺼번에 가져오려는 데이터의 타입을 입력해주세요.</typeparam>
        /// <returns></returns>
        public List<T> GetAssets<T>() where T : Object
        {
            List<T> result = new List<T>();

            foreach (var item in loadedAssets.Values)
            {
                if (item.targetAsset is T castedAsset)
                {
                    if (castedAsset is ScriptableObject)
                    {
                        T clone = Instantiate(castedAsset);
                        clonedSO.Add(clone as ScriptableObject);
                        result.Add(clone);
                    }

                    else result.Add(castedAsset);
                }
            }

            if (result.Count == 0)
            {
                Debug.LogWarning($"현재 메모리에 {typeof(T).Name} 타입으로 로드된 에셋이 없다...");
            }

            return result;
        }

        // 에셋 내리기
        public void UnloadAsset(string key)
        {
            foreach (var asset in loadedAssets.Values)
            {
                // 어드레서블이 로드했던 원본 객체 해제
                if (asset.originalAsset != null)
                {
                    Addressables.Release(asset.originalAsset);
                }

                // 생성된 Sprite 예외 처리
                if (asset.targetAsset is Sprite sprite)
                {
                    Destroy(sprite);
                }
            }

            // 생성된 SO 클론 해제
            foreach (var so in clonedSO)
            {
                if (so != null) Destroy(so);
            }

            loadedAssets.Clear();
            clonedSO.Clear();

            Debug.Log($"모든 에셋을 메모리에서 해제함!");
        }

        // Sprite의 경우 어드레서블에 등록된 것이 아니라 Texture2D에서 따로 생성해낸 데이터이기 때문에
        // 어드레서블 목록만 삭제할 경우 새로 생성된 Sprite는 같이 삭제되지 않고 GC에 남아있다.
        // Sprite도 같이 삭제해야 하므로 둘을 함께 관리하는 구조체를 명시한다.
        private struct AssetItem
        {
            public Object targetAsset;      // 인게임에서 사용할 에셋
            public Object originalAsset;    // 어드레서블 해제 시 사용할 원본 에셋
        }

    }

}
