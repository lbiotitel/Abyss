using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCamera : MonoBehaviour
{
    public float blockoutDistacne = 50f;

    private Renderer[] allRenderers;
    private MaterialPropertyBlock blackBlock;

    private void Start()
    {
        
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);

        if(distance > blockoutDistacne)
        {
            allRenderers = transform.GetComponentsInChildren<Renderer>();
            ApplyBlackout(true);
        }
        else
        {
            ApplyBlackout(false);
        }
    }

    private void ApplyBlackout(bool isBlack)
    {
        foreach (Renderer rend in allRenderers)
        {
            if (isBlack)
            {
                rend.SetPropertyBlock(blackBlock);
            }
            else
            {
                // null을 넣으면 원래 프리팹에 있던 원본 매터리얼 색상으로 즉시 돌아옵니다!
                rend.SetPropertyBlock(null);
            }
        }
    }
}
