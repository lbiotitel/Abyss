using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager 
{
    public InGameManager InGame { get; private set; }
    public CommodityManager Commodity { get; private set; }
    
    public void Init()
    {
        InGame = new InGameManager();
        Commodity = new CommodityManager();

        Managers.SoundManager.PlayBGM(KeyDefines.AudioKey.GAME_BGM);
        SceneTransition.IsSceneChanged += InGame.GameStart;

        if (SceneManager.GetActiveScene().name == "Biotite")
        {
            InGame.GameStart();
        }

        Commodity.Init();
    }

    public void Release() 
    {
        SceneTransition.IsSceneChanged -= InGame.GameStart;
    }
}
