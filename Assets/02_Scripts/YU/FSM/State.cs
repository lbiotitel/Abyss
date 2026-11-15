using UnityEngine;

public abstract class State<T> where T : Unit
{
    /// <summary>
    /// 해당 상태를 시작할 때 1회 호출
    /// </summary>
    /// <param name="entity"></param>
    public abstract void Enter(T entity);

    /// <summary>
    /// 해당 상태를 업데이트를 할 때 프레임 호출
    /// </summary>
    /// <param name="entity"></param>
    public abstract void Execute(T entity);
    
    /// <summary>
    /// 해당 상태를 종료할 때 1회 호출
    /// </summary>
    /// <param name="entity"></param>
    public abstract void Exit(T entity);

    /// <summary>
    /// 메시지를 받았을 때 1회 호출
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public abstract bool OnMessage(T entity, Telegram telegram);
}

