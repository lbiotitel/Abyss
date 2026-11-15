using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomPrefabMapping
{
    public Node.ENodeType nodeType;
    public GameObject prefab;
    [Range(0f, 100f)]
    public float weight; // 생성 확률 가중치 (비중)
}

[CreateAssetMenu(menuName = "Data/MapData")]
public class MapData : ScriptableObject
{
    public string mapName;  // 맵 이름

    public int roomSize = 44;
    public int width;       // 너비
    public int height;      // 높이
    public int reqLevel;    // 해금 레벨
    [Header("맵의 최대값(너비 or 높이)을 적으면 됩니다")]
    public int maxRooms = 4;

    [Header("방 설정")]
    [Tooltip("노드 타입별로 생성할 프리팹을 지정하세요.")]
    public List<RoomPrefabMapping> roomMaps;

    public AudioClip BGM;
    public List<EnemySpawnPoint> enemySpawn;

    public List<string> itemCodes; // 보상이 될 아이템들
}
