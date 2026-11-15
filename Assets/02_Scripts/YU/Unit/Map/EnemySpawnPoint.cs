using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnPoint
{
    public EnemyData enemyData;

    public Vector3 Position; // 적이 스폰될 위치

    public int maxCount = 1;
}
