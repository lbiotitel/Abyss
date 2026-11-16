using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public abstract class EffectExecution
{
    public abstract void ExecuteEffect(Unit source, Unit target);
    public abstract void StopEffect();
}
