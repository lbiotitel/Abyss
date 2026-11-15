using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class PerformBootStrap
{
    const string SceneName = "BootStrap";

    // Awake 함수 실행 전에 씬 로드
    // static 메서드에서만 적용 가능
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static public void InitializeLoad()
    {
        bool isLoaded = false;

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == SceneName)
            {
                isLoaded = true;
                break;
            }
        }

        if (!isLoaded)
        {
            SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
            Debug.Log($"{SceneName} 씬을 하이어라키에 추가함!");
        }

        // 매니저 초기화
    }
}