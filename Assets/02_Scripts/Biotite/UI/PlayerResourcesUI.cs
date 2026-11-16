using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using TMPro;
using UnityEngine;



//인게임에서 플레이어가 가지고 있는 재화를 표시합니다.
public class PlayerResourcesUI : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText;

    private void Start()
    {
        Managers.Event.AddEvent<OnInGameStart>(HandleOnInGameStart);
       
    }

    private void HandleOnInGameStart(OnInGameStart e)
    {
         Managers.Game.InGame.OnGoldChanged += HandleGoldChanged;
         Managers.Event.RemoveEvent<OnInGameStart>(HandleOnInGameStart);
    }


    private void OnDestroy()
    {
        Managers.Game.InGame.OnGoldChanged -= HandleGoldChanged;
    }


    private void HandleGoldChanged(int gold)
    {
        goldText.text = $"{gold}G"; 
    }
}
