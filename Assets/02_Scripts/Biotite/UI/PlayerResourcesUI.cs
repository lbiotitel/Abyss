using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using TMPro;
using UnityEngine;



//�ΰ��ӿ��� �÷��̾ ������ �ִ� ��ȭ�� ������ �մϴ�.
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
