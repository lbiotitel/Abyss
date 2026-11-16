using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//파티클들을 풀링합니다.
public class PoolingParticle : MonoBehaviour,IPoolingObject
{
    public GameObject OriginPrefab { get; set; }
    private ParticleSystem particle;

    private void Start()
    {
        particle = GetComponent<ParticleSystem>();
        var mainP = particle.main;
        mainP.stopAction = ParticleSystemStopAction.Callback;
    }

    private void OnParticleSystemStopped()
    {
        ReturnObject();   
    }

    public void ResetObject()
    {
        particle.Stop();
    }

    public void ReturnObject()
    {
        ResetObject();
        Managers.Object.PoolingSystem.ReturnObject(gameObject);
    }
}
