using System;
using UnityEngine;





//장비의 모션을 정의하는 클래스 
[Serializable]
public abstract class WeaponMotionBase : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected string[] attackAnimName;
    [SerializeField] protected string parryingAnimName;

    public abstract void AttackMotion();
    public abstract void ParryingMotion();
}
