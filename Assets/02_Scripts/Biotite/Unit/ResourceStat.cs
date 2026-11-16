using UnityEngine;



//hp,mp등 사용하는 스탯에 대한 정의 입니다.
public class ResourceStat : StatBase
{
    public ResourceStat(float value, EStatType statType) : base(value, statType)
    {
    }

    public ResourceStat(float value, EStatType statType, StatBase linkStat) : base(value, statType)
    {
        baseValue = value;
        maxValue = baseValue;
        linkStat.OnStatValueChanged += HandleLinkValueChnaged;
    }


    private void HandleLinkValueChnaged(EStatType statType, float prevValue, float newValue)
    {
        maxValue = newValue;
        float prev = baseValue;
        baseValue = Mathf.Clamp(baseValue,0,maxValue);
        TryInvokeOnValueChanged(prev, baseValue);
    }


    public override void AddModifier(StatModifier statModifier)
    {
        float prevValue = GetValue();
        baseValue += statModifier.Value;
        baseValue = Mathf.Min(baseValue, maxValue);
        TryInvokeOnValueChanged(prevValue, baseValue);
    }

    public override void RemoveModifier(StatModifier statModifier)
    {
        float prevValue = GetValue();
        baseValue -= statModifier.Value;
        baseValue = Mathf.Max(baseValue, 0);
        TryInvokeOnValueChanged(prevValue, baseValue);
    }

    public override float GetValue()
        => baseValue;

}
