using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public abstract class EffectExcution
{
    public abstract void ExcuteEffect(Unit source, Unit target);
    public abstract void StopEffect();
}
