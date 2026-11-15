using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapGenerator : MonoBehaviour
{
    private MapData data;

    [Header("안전 장치")]
    [Tooltip("BFS 반복 횟수 상한. 이 값을 초과하면 현재까지 결과를 유지하고 종료합니다.")]
    public int maxIterations = 2000;

    private Node[,] _nodeGrid;
    private bool[,] _isOccupied;
    private List<Node> _allNodes;
    private Vector2Int _startGridIndex;

    private static readonly int[] DirX = { 0, 0, -1, 1 };
    private static readonly int[] DirY = { 1, 0, 0, 0 };

    [SerializeField] private Node.ENodeType curRoom;
    [SerializeField] private int CurrentNodeIndex;

    private void Start()
    {
        Managers.Event.AddEvent<OnInGameStart>(HandleOnInGameStart);
        Managers.Event.AddEvent<OnNextRoom>(MoveNextRoom);
    }

    private void OnDisable()
    {
        Managers.Event.RemoveEvent<OnInGameStart>(HandleOnInGameStart);
        Managers.Event.RemoveEvent<OnNextRoom>(MoveNextRoom);
    }

    private void HandleOnInGameStart(OnInGameStart e) => Run();

    private void MoveNextRoom(OnNextRoom e)
    {
        // 0번 행에 일렬로 배치되었다고 가정하는 기존 로직 유지
        int nextIndex = CurrentNodeIndex + (int)e.moveDir;
        if (nextIndex < 0 || nextIndex >= _allNodes.Count) return;

        CurrentNodeIndex = nextIndex;
        curRoom = _allNodes[CurrentNodeIndex].nodeType;

        switch (curRoom)
        {
            case Node.ENodeType.Monster:
                SpawnMonsters();
                break;
            case Node.ENodeType.Exit:
                Managers.Game.InGame.GameOver();
                break;
        }
    }

    private void SpawnMonsters()
    {
        int r = Random.Range(0, data.enemySpawn.Count);

        int rc = Random.Range(1, data.enemySpawn[r].maxCount + 1);

        Vector3 worldPos = _allNodes[CurrentNodeIndex].mapObject.transform.position + data.enemySpawn[r].Position;

        List<Unit> list = new List<Unit>();
        for (int i = 0; i < rc; i++)
        {
            Unit unit = Managers.Object.CreateUnit(data.enemySpawn[r].enemyData, worldPos);
            float xpos = (i - (rc - 1) * 0.5f) * unit.GetComponent<SkinnedMeshRenderer>().bounds.size.x * 1.2f;
            unit.transform.position += new Vector3(xpos, -0.5f, 0);
            list.Add(unit);
        }
        Managers.Event.NotifyEvent(new OnSpawnUnit { unitList = list });
    }

    [ContextMenu("맵 생성")]
    public void Run()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        data = Managers.Resource.GetAsset<MapData>("Stage0");
        
        ClearOldRooms();

        bool completed = TryGenerateMapData();
        SpawnRoomPrefabs();

        if (completed) Debug.Log($"<color=cyan><b>[MapGenerator]</b> 완료 — 방 {_allNodes.Count}개</color>");
        PrintGridToConsole();
    }

    public bool TryGenerateMapData()
    {
        _nodeGrid = new Node[data.width, data.height];
        _isOccupied = new bool[data.width, data.height];
        _allNodes = new List<Node>();
        CurrentNodeIndex = 0;

        // 시작 노드 생성
        Node start = CreateAndRegisterNode(0, 0, Node.ENodeType.Start);
        _startGridIndex = new Vector2Int(0, 0);

        Queue<Vector2Int> expansionQueue = new Queue<Vector2Int>();
        expansionQueue.Enqueue(_startGridIndex);

        List<Node> bfsOrder = new List<Node>();
        int iterationCount = 0;

        while (expansionQueue.Count > 0 && _allNodes.Count < data.maxRooms)
        {
            if (++iterationCount > maxIterations) return false;

            Vector2Int current = expansionQueue.Dequeue();
            Node currentNode = _nodeGrid[current.x, current.y];

            foreach (int dir in ShuffledDirectionIndices())
            {
                if (_allNodes.Count >= data.maxRooms) break;

                int nx = current.x + DirX[dir];
                int ny = current.y + DirY[dir];

                if (!IsInsideGrid(nx, ny) || _isOccupied[nx, ny]) continue;
                if (CountOccupiedNeighbors(nx, ny) > 1) continue;
                if (Random.value < 0.3f) continue;

                Node newNode = CreateAndRegisterNode(nx, ny, Node.ENodeType.None);
                LinkNodes(currentNode, newNode);
                bfsOrder.Add(newNode);
                expansionQueue.Enqueue(new Vector2Int(nx, ny));
            }

            if (expansionQueue.Count == 0 && _allNodes.Count < data.maxRooms && _allNodes.Count > 0)
            {
                Node retryNode = _allNodes[Random.Range(0, _allNodes.Count)];
                expansionQueue.Enqueue(retryNode.pos);
            }
        }

        if (bfsOrder.Count == 0) return false;

        // 마지막 노드는 Exit으로 설정
        Node exitNode = bfsOrder[bfsOrder.Count - 1];
        exitNode.nodeType = Node.ENodeType.Exit;

        // [수정] 중간 노드들은 가중치에 따라 결정
        for (int i = 0; i < bfsOrder.Count - 1; i++)
        {
            bfsOrder[i].nodeType = GetRandomTypeByWeight();
        }

        return true;
    }

    // [수정] 가중치 기반 랜덤 타입 선택
    private Node.ENodeType GetRandomTypeByWeight()
    {
        var validMappings = data.roomMaps.Where(m => m.weight > 0 &&
                                               m.nodeType != Node.ENodeType.Start &&
                                               m.nodeType != Node.ENodeType.Exit).ToList();

        if (validMappings.Count == 0) return Node.ENodeType.Monster;

        float totalWeight = validMappings.Sum(m => m.weight);
        float pivot = Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        foreach (var mapping in validMappings)
        {
            currentWeight += mapping.weight;
            if (pivot <= currentWeight) return mapping.nodeType;
        }

        return Node.ENodeType.Monster;
    }

    // [수정] 프리팹 배치 로직 (Start/Exit 분기 처리)
    private void SpawnRoomPrefabs()
    {
        foreach (Node node in _allNodes)
        {
            // 규칙: Start 혹은 Exit 타입 노드라면 'None' 타입의 프리팹을 사용함
            Node.ENodeType targetPrefabType = node.nodeType;
            if (node.nodeType == Node.ENodeType.Start || node.nodeType == Node.ENodeType.Exit)
            {
                targetPrefabType = Node.ENodeType.None;
            }

            RoomPrefabMapping mapping = data.roomMaps.Find(m => m.nodeType == targetPrefabType);

            if (mapping != null && mapping.prefab != null)
            {
                Vector3 worldPosition = GridToWorldPosition(node.pos);
                GameObject roomObject = Instantiate(mapping.prefab, worldPosition, Quaternion.identity);

                node.mapObject = roomObject;
                roomObject.transform.SetParent(transform);
                roomObject.name = $"Room_{node.nodeType}_{node.pos.x}_{node.pos.y}";
                
                // 필요 시 색상 적용
                ApplyRoomColor(roomObject, node.nodeType);
            }
            else
            {
                Debug.LogWarning($"[MapGenerator] {targetPrefabType} 타입에 대한 프리팹 매핑을 찾을 수 없습니다.");
            }
        }
    }

    private Node CreateAndRegisterNode(int x, int y, Node.ENodeType type)
    {
        Node node = new Node { pos = new Vector2Int(x, y), nodeType = type, nextNode = new List<Node>() };
        _nodeGrid[x, y] = node;
        _isOccupied[x, y] = true;
        _allNodes.Add(node);
        return node;
    }

    private void LinkNodes(Node a, Node b)
    {
        if (!a.nextNode.Contains(b)) a.nextNode.Add(b);
        if (!b.nextNode.Contains(a)) b.nextNode.Add(a);
    }

    private Vector3 GridToWorldPosition(Vector2Int gridPos)
    {
        float x = (gridPos.x - _startGridIndex.x) * data.roomSize;
        float z = (gridPos.y - _startGridIndex.y) * data.roomSize;
        return new Vector3(x, 0f, z);
    }

    private bool IsInsideGrid(int x, int y) => x >= 0 && x < data.width && y >= 0 && y < data.height;

    private int CountOccupiedNeighbors(int x, int y)
    {
        int count = 0;
        for (int i = 0; i < 4; i++)
        {
            int nx = x + DirX[i];
            int ny = y + DirY[i];
            if (IsInsideGrid(nx, ny) && _isOccupied[nx, ny]) count++;
        }
        return count;
    }

    private int[] ShuffledDirectionIndices()
    {
        int[] indices = { 0, 1, 2, 3 };
        for (int i = 0; i < indices.Length; i++)
        {
            int swapTarget = Random.Range(0, indices.Length);
            int temp = indices[i];
            indices[i] = indices[swapTarget];
            indices[swapTarget] = temp;
        }
        return indices;
    }

    private void ApplyRoomColor(GameObject roomObject, Node.ENodeType type)
    {
        Renderer roomRenderer = roomObject.GetComponent<Renderer>();
        if (roomRenderer == null) return;

        roomRenderer.material.color = type switch
        {
            Node.ENodeType.Start => Color.cyan,
            Node.ENodeType.Monster => Color.white,
            Node.ENodeType.Treasure => Color.yellow,
            Node.ENodeType.Event => Color.red,
            Node.ENodeType.Exit => Color.magenta,
            _ => Color.white
        };
    }

    private void ClearOldRooms()
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in transform) children.Add(child.gameObject);
        foreach (var child in children) DestroyImmediate(child);
    }

    public void PrintGridToConsole()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"[MapGenerator] 그리드 맵 ({data.width}x{data.height})");
        for (int y = data.height - 1; y >= 0; y--)
        {
            for (int x = 0; x < data.width; x++)
            {
                if (!_isOccupied[x, y]) { sb.Append(" . "); continue; }
                Node node = _nodeGrid[x, y];
                string code = node.nodeType.ToString().Substring(0, 1);
                sb.Append($"[{code}]");
            }
            sb.AppendLine();
        }
        Debug.Log(sb.ToString());
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space(10);
        MapGenerator generator = (MapGenerator)target;
        GUI.enabled = !Application.isPlaying;
        if (GUILayout.Button("🗺  맵 생성", GUILayout.Height(36)))
        {
            Undo.RegisterFullObjectHierarchyUndo(generator.gameObject, "Generate Map");
            generator.Run();
        }
        GUI.enabled = true;
    }
}
#endif