using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class CandleLight : MonoBehaviour
{

    [SerializeField] private float frequency;
    [SerializeField] private float blinkingSize;
    private float originIntensity;
    private Light pointLight;

    private void Start()
    {
        pointLight = GetComponent<Light>();
        originIntensity = pointLight.intensity;
    }


    private void Update()
    {
        pointLight.intensity = originIntensity + Mathf.Cos(Time.time * frequency * Mathf.PI * 2) * blinkingSize;
    }

}
