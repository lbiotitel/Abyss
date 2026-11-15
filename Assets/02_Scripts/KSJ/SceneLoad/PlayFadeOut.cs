using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayFadeOut : StateMachineBehaviour
{
    [SerializeField] private SceneTransition sceneController;
    [SerializeField] private SceneTransition.TargetSceneName targetScene;
    private bool isChanged = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(stateInfo.normalizedTime >= 1f && isChanged == false)
        {
            Debug.Log($"애니메이터의 출처는~~ {animator.gameObject.name}");

            sceneController = animator.GetComponentInParent<SceneTransition>(true);
            targetScene = sceneController.targetSceneNumber;

            if (sceneController != null)
            {
                sceneController.LoadAndSetActiveScene(targetScene).Forget();
            }

            else
            {
                Debug.Log($"{sceneController} 할당 안됐대요~!");
            }

            isChanged = true;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
   /*
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
    */

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
