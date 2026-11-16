using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventNotifier : MonoBehaviour
{
    public event Action<EAnimationTriggerType> OnAnimTrigger;


    //애니메이션 이벤트 트리거
    public void AnimFinAniished(EAnimationTriggerType animationTriggerType)
        => OnAnimTrigger?.Invoke(animationTriggerType); 
    
}
