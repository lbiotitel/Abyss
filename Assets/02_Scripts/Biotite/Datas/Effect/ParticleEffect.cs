using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public enum EParticleSpawnPos
{ 
    OWNPOS,
    TARGETPOS
}


//파티클을 생성하는 이펙트 입니다.
[Serializable]
public class ParticleEffect : EffectBase
{
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private EParticleSpawnPos spawnPos;


    public override void Execute(Unit source, Unit target)
    {
        GameObject particle = Managers.Object.PoolingSystem.GetObject(particlePrefab,Vector3.zero,Quaternion.identity);

        if (spawnPos == EParticleSpawnPos.OWNPOS)
        {
            particle.transform.position = source.transform.position; ;
        }
        else if (spawnPos == EParticleSpawnPos.TARGETPOS)
        {
            particle.transform.position = target.transform.position;
        }

    }

    public override void StopEffect()
    {
       
    }
}
