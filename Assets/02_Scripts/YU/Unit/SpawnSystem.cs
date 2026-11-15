using UnityEngine;

public class SpawnSystem
{
    public SpawnSystem()
    {
        Managers.Event.AddEvent<OnSpawnUnit>(Spawn);
    }

    /// <summary>
    /// 외부(이벤트 등)에서 유닛을 생성할 때 호출하는 메서드
    /// </summary>
    public void Spawn(OnSpawnUnit e)
    {
        Managers.Battle.Setup(e.unitList);
    }
}
