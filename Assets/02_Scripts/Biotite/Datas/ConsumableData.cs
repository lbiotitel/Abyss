using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//소모품 데이터

[CreateAssetMenu]
public class ConsumableData : ItemDataBase
{

    private void Awake()
    {
        itemType = EItemType.Consumable;
    }

    [Header("소모품 속성")]
    [SerializeField] private bool isPermanent = false;
    [Header("소모품 아이템 효과")]
    [SerializeReference,SubclassSelector] private List<EffectExecution> effectExecutions;

    public List<EffectExecution> EffectExecutions => effectExecutions;
    public bool IsPermanent => isPermanent; 
}
