using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using System;

public class SceneTransition : MonoBehaviour
{
    public enum TargetSceneName { Tycoon = 1, Dungeon = 2, }
    public TargetSceneName targetSceneNumber;
    [SerializeField] private GameObject currSceneObj;

    [Header("Fade 연출")]
    [SerializeField] private Animator animator;

    private Vector3 originPosition;

    // Scene이 바뀌자마자 호출
    static public Action canObjectToggle;
    // Fade 연출 후 호출
    static public Action IsSceneChanged;

    private void Awake()
    {
        if(currSceneObj != null)
        {
            originPosition = currSceneObj.transform.position;
        }
    }

    public void RunFadein()
    {
        animator.gameObject.SetActive(true);
    }

    /// <summary>
    /// 영지에서 던전으로 씬 전환 시 사용
    /// 영지: additive, 던전: 액티브 설정
    /// </summary>
    /// <param name="sceneNumber"></param>
    /// <returns></returns>
    public async UniTask LoadAndSetActiveScene(TargetSceneName sceneNumber)
    {
        // enum을 빌드 인덱스로 변환
        int sceneBuildIndex = (int)sceneNumber;

        // 현재 하이어라키에 열린 씬을 모두 조사
        Scene targetScene = new Scene();
        bool isAlreadyLoaded = false;

        for(int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene loadedScene = SceneManager.GetSceneAt(i);

            if(loadedScene.buildIndex == sceneBuildIndex)
            {
                targetScene = loadedScene;
                isAlreadyLoaded = true;
                break;
            }
        }

        // 만약 씬이 이미 로드되어 있다면 바로 로드
        if(isAlreadyLoaded == true && targetScene.IsValid() && targetScene.isLoaded)
        {
            bool success = SceneManager.SetActiveScene(targetScene);
            canObjectToggle?.Invoke();
            RunFadeout();
            SwitchPosition(targetScene);
            Debug.Log($"이미 로드된 {sceneNumber} 씬을 Active로 설정함! 결과: {success}");
            return;
        }

        // 로드가 완료될 때까지 대기
        Debug.Log($"{targetScene.name} 씬 로드 시작!");
        await SceneManager.LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Additive).ToUniTask();

        targetScene = SceneManager.GetSceneByBuildIndex(sceneBuildIndex);

        if (targetScene.IsValid() && targetScene.isLoaded)
        {

            bool success = SceneManager.SetActiveScene(targetScene);
            canObjectToggle?.Invoke();
            RunFadeout();
            Debug.Log($"새로 로드된 {sceneNumber} 씬을 Active로 설정함! 결과: {success}");

            if (success) SwitchPosition(targetScene);
        }
    }

    /// <summary>
    /// 던전에서 영지로 씬 전환 시 사용
    /// 던전: 언로드, 영지: 액티브 설정
    /// </summary>
    /// <param name="unloadSceneNumber"></param>
    /// <param name="returnSceneNumber"></param>
    public async void UnloadScene(TargetSceneName unloadSceneNumber, TargetSceneName returnSceneNumber)
    {
        int unloadSceneIndex = (int)unloadSceneNumber;
        int returnSceneIndex = (int)returnSceneNumber;

        // 돌아갈 씬이 하이어라키에 등록되어 있는지 확인 뒤 액티브로 설정
        Scene returnScene = new Scene();
        bool isLoaded = false;

        for(int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene loadedScene = SceneManager.GetSceneAt(i);

            if(loadedScene.buildIndex == returnSceneIndex)
            {
                returnScene = loadedScene;
                isLoaded = true;
                break;
            }
        }

        if(isLoaded && returnScene.IsValid() && returnScene.isLoaded)
        {
            // 영지 씬을 먼저 액티브로 설정
            SceneManager.SetActiveScene(returnScene);

            // 1000, 1000, 0으로 옮겨놨던 오브젝트 위치 원상복귀
            SwitchPosition(returnScene);
        }

        else
        {
            Debug.LogError("영지 씬이 지워졌다!! 오류!!");
        }

        // 던전 씬을 비동기로 언로드
        // 여기서 언로드하면 페이드 연출이랑 씬 바뀜 신호를 다 못받기 때문에
        // 영지씬 액티브 > 신호주기 > 언로드
        canObjectToggle?.Invoke();
        await SceneManager.UnloadSceneAsync(unloadSceneIndex).ToUniTask();
        Debug.Log($"던전 씬 언로드 완료!");
    }

    private void RunFadeout()
    {
        animator.SetBool("isChanged", true);
    }

    public void SwitchPosition(Scene targetScene)
    {
        if (currSceneObj == null) return;

        Debug.Log("위치바꿔!!!");
        if (targetScene.IsValid() && targetScene.isLoaded)
        {
            if (SceneManager.GetActiveScene() == targetScene)
            {
                if(targetScene.buildIndex == (int)TargetSceneName.Dungeon)
                {
                    Transform tf = currSceneObj.transform;
                    tf.position = new Vector3(1000, 1000, tf.position.z);
                }

                else
                {
                    currSceneObj.transform.position = originPosition;
                }
            }
        }
    }

    public void NoticeSceneChange()
    {
        IsSceneChanged?.Invoke();
        canObjectToggle?.Invoke();
        Debug.Log("씬 바꼈다~~");
    }
}
