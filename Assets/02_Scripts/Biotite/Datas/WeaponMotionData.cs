using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class WeaponMotionData : ScriptableObject
{
    [SerializeField] private RuntimeAnimatorController weaponAnimator;
    [SerializeField] private string idleAnimName;
    [SerializeField] private string[] atkAnimNames;
    [SerializeField] private string parryingAnimName;

    public RuntimeAnimatorController WeaponAnimator => weaponAnimator;
    public string IdleAnimName => idleAnimName;
    public string[] AtkAnimNames => atkAnimNames;
    public string ParryingAnimName => parryingAnimName;
}
