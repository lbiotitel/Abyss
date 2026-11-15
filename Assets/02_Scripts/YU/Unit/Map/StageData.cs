using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/StageData")]
public class StageData : ScriptableObject
{
    [System.Serializable]
    public struct EnemyDataList
    {
        public List<string> enemyNameList;
    }

    [SerializeField] private string stageName;
    [SerializeField] List<GameObject> maps;
    [SerializeField] private string bossName;
    [SerializeField] private List<EnemyDataList> stageEnemyList;
    

    public string StageName => stageName;
    public List<GameObject> Maps => maps;
    public string BossName => bossName;
    public List<EnemyDataList> StageEnemyList => stageEnemyList;
}
