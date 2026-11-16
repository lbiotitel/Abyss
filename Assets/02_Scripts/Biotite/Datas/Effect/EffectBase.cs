using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//데미지 효과 등 여러 이펙트를 구현합니다.
[Serializable]
public abstract class EffectBase   
{
    public abstract void Execute(Unit source, Unit target);
    public abstract void StopEffect();
}
