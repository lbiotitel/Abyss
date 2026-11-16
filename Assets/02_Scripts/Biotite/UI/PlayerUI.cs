using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;


//플레이어의 mp hp의 ui를 정의합니다.
public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Image hpSlider;
    [SerializeField] private Image hpBgSlider;
    [SerializeField] private Image mpSlider;
    [SerializeField] private Image mpBgSlider;

    [SerializeField] private float animDuration;
    [SerializeField] private float bgAnimDuration;

    private UnitStatContainer statContainer;

    public void Init(UnitStatContainer unitStat)
    {
        statContainer = unitStat;
        unitStat.OnStatValueChanged += HandleStatChanged;
    }


    private void HandleStatChanged(EStatType statType, float oldValue, float newValue)
    { 
        if (statType == EStatType.CurrentHP || statType == EStatType.MaxHp)
        {
            float maxHp = statContainer.GetStatValue(EStatType.MaxHp);
            float currentHp = statContainer.GetStatValue(EStatType.CurrentHP);
            hpSlider.DOFillAmount(currentHp / maxHp, animDuration);
            hpBgSlider.DOFillAmount(currentHp / maxHp, bgAnimDuration);
        }

        if (statType == EStatType.CurrentMP || statType == EStatType.MaxMP)
        {
            float maxMp = statContainer.GetStatValue(EStatType.MaxMP);
            float currentMp = statContainer.GetStatValue(EStatType.CurrentMP);
            mpSlider.DOFillAmount(currentMp / maxMp, animDuration);
            mpBgSlider.DOFillAmount(currentMp / maxMp, bgAnimDuration);
        }
    }
}
