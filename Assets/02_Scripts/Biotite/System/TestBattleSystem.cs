using System.Collections.Generic;
using UnityEngine;



//�׽�Ʈ�� ���� ��Ʋ �ý��� �Դϴ�.
public class TestBattleSystem : MonoBehaviour
{
    [SerializeField] private Transform spawnPos;
    [SerializeField] private UnitData enemyData;
    public List<Unit> enemies = new List<Unit>();
    float originY = 0.5f;

    private void Start()
    {
        //Init();
    }

    public void Init()
    {
        for (int i = -1; i <= 1; i++)
        {
           /* Unit enemy = Managers.Object.CreateUnit(enemyData);
            enemy.transform.position = spawnPos.transform.position + new Vector3(i * 2,originY,0);
            enemies.Add(enemy);*/
        }
    }


    private void Update()
    {
        if (enemies == null) return;
        foreach (var enemy in enemies)
        {
            float newY = originY + Mathf.Sin(Time.time * 3f) * 0.2f;

            enemy.transform.position = new Vector3(
                enemy.transform.position.x,
                newY,
                enemy.transform.position.z
            );
        }
    }

    public Unit GetFrontEnemy()
    {
        if (enemies.Count == 0) return null;
        return enemies[0];
    }


    public List<Enemy> GetAllEnemies()
    { 
        List<Enemy> res = new List<Enemy>();

        foreach (var enemy in enemies)
        {
            res.Add(enemy as Enemy);
        }

        return res;
    }

}
