
using Unity.VisualScripting;
using UnityEngine.XR;

public class StateMachine<T> where T : Unit
{
    private T ownerEntity;          // StateMachine의 소유주
    private State<T> currentState;  // 현재 상태
    private State<T> previousState; // 이전 상태
    private State<T> globalState;   // 전역상태

    private bool forceExitRequested;

    private bool isStop = false;

    public void Setup(T owner, State<T> entryState)
    {
        ownerEntity = owner;
        currentState = null;
        previousState = null;
        globalState = null;

        ChangeState(entryState);
    }

    public void Execute()
    {
        if (isStop) return;

        // 글로벌 상태를 업데이트
        if (globalState != null)
        {
            globalState.Execute(ownerEntity);
        }

        // 현재 상태를 업데이트
        if (currentState != null)
        {
            currentState.Execute(ownerEntity);
        }
    }

    public void ChangeState(State<T> newState)
    {
        // 현재 재생중인 상태가 있으면 Exit() 메소드 호출
        if (currentState != null)
        {
            // 상태가 변경되면 현재 상태는 이전 상태가 되기 때문에 previousState에 저장
            //previousState = currentState;

            currentState.Exit(ownerEntity);
        }

        // 새로운 상태로 변경하고, 새로 바뀐 상태의 Enter() 메소드 호출
        currentState = newState;
        currentState.Enter(ownerEntity);
    }
    
    public void SetGlobalState(State<T> newState)
    {
        globalState = newState;
    }

    public void RevertToPreviousState(State<T> newState)
    {
        ChangeState(previousState);
    }

    public void RequestForceExit()
    {
        forceExitRequested = true;
    }

    public bool HandleMessage(Telegram telegram)
    {
        if(globalState != null && globalState.OnMessage(ownerEntity, telegram))
        {
            return true;
        }
        
        if(currentState != null && currentState.OnMessage(ownerEntity, telegram))
        {
            return true;
        }

        return false;
    }

    public void StopStateMahine()
    {
        isStop = true;
    }
}