using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneObjectToggle : MonoBehaviour
{
    [SerializeField] private GameObject[] toggleObjects;
    [SerializeField] private GameObject domCamera;
    
    [SerializeField] private SceneTransition.TargetSceneName parentSceneName = SceneTransition.TargetSceneName.Tycoon;
    
    private Scene savedScene;
    private int sceneIndex;

    private void OnEnable()
    {
        SceneTransition.canObjectToggle += DoToggle;
        ObjectChange.OnCameraChanged += CameraAction;
    }

    private void DoToggle()
    {
        sceneIndex = (int)parentSceneName;
        savedScene = SceneManager.GetActiveScene();
        Debug.Log($"Áö±Ý ¾À: {savedScene.name}");

        if (savedScene.buildIndex != sceneIndex)
        {
            for(int i = 0; i < toggleObjects.Length; i++)
            {
                toggleObjects[i].SetActive(false);
            }
        }

        else
        {
            for (int i = 0; i < toggleObjects.Length; i++)
            {
                toggleObjects[i].SetActive(true);
            }
        }
    }

    public void CameraAction()
    {
        domCamera.SetActive(true);
    }

    private void OnDisable()
    {
        SceneTransition.canObjectToggle -= DoToggle;
        ObjectChange.OnCameraChanged -= CameraAction;
    }

}
