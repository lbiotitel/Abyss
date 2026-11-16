using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInfoUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text maxHpText;
    [SerializeField] private TMP_Text currentHpText;
    [SerializeField] private TMP_Text maxMpText;
    [SerializeField] private TMP_Text currentMpText;
    [SerializeField] private TMP_Text defText;
    [SerializeField] private TMP_Text atkText;
    [SerializeField] private TMP_Text atkSpeedText;
    [SerializeField] private TMP_Text strText;
    [SerializeField] private TMP_Text dexText;
    [SerializeField] private TMP_Text intText;
    [SerializeField] private TMP_Text luckText;
    [SerializeField] private TMP_Text criRateText;
    [SerializeField] private TMP_Text criDamageText;
    [SerializeField] private TMP_Text evasionRateText;
    [SerializeField] private TMP_Text penetrationRateText;
    [SerializeField] private TMP_Text damageCorrectionText;
    [SerializeField] private TMP_Text poisonResistanceText;
    [SerializeField] private TMP_Text igniteResistanceText;
    [SerializeField] private TMP_Text bleedResistanceText;
    [SerializeField] private TMP_Text frozenResistanceText;

    [SerializeField] private GameObject contentTransform;
    private UnitStatContainer unitStatContainer;

    public void Init(UnitStatContainer unitStatContainer)
    {
        this.unitStatContainer = unitStatContainer;
        unitStatContainer.OnStatValueChanged += HandleOnStatValueChanged;
        RefreshAllUI();
        contentTransform.SetActive(false);
    }

    private void OnDestroy()
    {
        if (unitStatContainer != null)
        {
            unitStatContainer.OnStatValueChanged -= HandleOnStatValueChanged;
        }
    }


    private void Update()
    {
        if (Managers.Input == null) return;
        if (Managers.Input.StatButton)
        {
            contentTransform.SetActive(!contentTransform.activeSelf);
        }
    }


    //초기의 UI 세팅을 합니다
    private void RefreshAllUI()
    {
        if (unitStatContainer == null) return;

        maxHpText.text = $"{unitStatContainer.GetStatValue(EStatType.MaxHp)}";
        currentHpText.text = $"{unitStatContainer.GetStatValue(EStatType.CurrentHP)}";
        maxMpText.text = $"{unitStatContainer.GetStatValue(EStatType.MaxMP)}";
        currentMpText.text = $"{unitStatContainer.GetStatValue(EStatType.CurrentMP)}";
        defText.text = $"{unitStatContainer.GetStatValue(EStatType.DEF)}";
        atkText.text = $"{unitStatContainer.GetStatValue(EStatType.ATK)}";
        atkSpeedText.text = $"{unitStatContainer.GetStatValue(EStatType.AtkSpeed)}";
        strText.text = $"{unitStatContainer.GetStatValue(EStatType.Str)}";
        dexText.text = $"{unitStatContainer.GetStatValue(EStatType.Dex)}";
        intText.text = $"{unitStatContainer.GetStatValue(EStatType.Int)}";
        luckText.text = $"{unitStatContainer.GetStatValue(EStatType.Luck)}";

        criRateText.text = $"{unitStatContainer.GetStatValue(EStatType.Cri)}%";
        criDamageText.text = $"{unitStatContainer.GetStatValue(EStatType.CriDamage)}%";
        evasionRateText.text = $"{unitStatContainer.GetStatValue(EStatType.EvasionRate)}%";
        penetrationRateText.text = $"{unitStatContainer.GetStatValue(EStatType.PenetrationRate)}%";
        igniteResistanceText.text = $"{unitStatContainer.GetStatValue(EStatType.IgniteResistance)}%";
        poisonResistanceText.text = $"{unitStatContainer.GetStatValue(EStatType.PoisonResistance)}%";
        frozenResistanceText.text = $"{unitStatContainer.GetStatValue(EStatType.FrozenResistance)}%";
        bleedResistanceText.text = $"{unitStatContainer.GetStatValue(EStatType.BleedResistance)}%";
        damageCorrectionText.text = $"{unitStatContainer.GetStatValue(EStatType.DamageCorrection)}%";
    }


    //스탯이 변경될 때를 감지하여 특정 스탯을 업데이트 합니다.
    private void HandleOnStatValueChanged(EStatType statType, float oldValue, float newValue)
    {
        switch (statType)
        {
            case EStatType.MaxHp: maxHpText.text = $"{newValue}"; break;
            case EStatType.CurrentHP: currentHpText.text = $"{newValue}"; break;
            case EStatType.MaxMP: maxMpText.text = $"{newValue}"; break;
            case EStatType.CurrentMP: currentMpText.text = $"{newValue}"; break;
            case EStatType.DEF: defText.text = $"{newValue}"; break;
            case EStatType.ATK: atkText.text = $"{newValue}"; break;
            case EStatType.AtkSpeed: atkSpeedText.text = $"{newValue}"; break;
            case EStatType.Str: strText.text = $"{newValue}"; break;
            case EStatType.Dex: dexText.text = $"{newValue}"; break;
            case EStatType.Int: intText.text = $"{newValue}"; break;
            case EStatType.Luck: luckText.text = $"{newValue}"; break;

            case EStatType.Cri: criRateText.text = $"{newValue}%"; break;
            case EStatType.CriDamage: criDamageText.text = $"{newValue}%"; break;
            case EStatType.EvasionRate: evasionRateText.text = $"{newValue}%"; break;
            case EStatType.PenetrationRate: penetrationRateText.text = $"{newValue}%"; break;
            case EStatType.IgniteResistance: igniteResistanceText.text = $"{newValue}%"; break;
            case EStatType.PoisonResistance: poisonResistanceText.text = $"{newValue}%"; break;
            case EStatType.FrozenResistance: frozenResistanceText.text = $"{newValue}%"; break;
            case EStatType.BleedResistance: bleedResistanceText.text = $"{newValue}%"; break;
            case EStatType.DamageCorrection: damageCorrectionText.text = $"{newValue}%"; break;
        }
    }
}
