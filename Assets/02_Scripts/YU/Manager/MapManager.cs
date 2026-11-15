using UnityEngine;

public class MapManager
{

    // 플레이어가 가고자 하는 방향을 매개변수로 받아 이동 가능 여부 확인 후 결과 노드 반환
    //public Node GetNextNodeIfValid(Vector2Int moveDir)
    //{
    //    if (grid == null) return null;

    //    Vector2Int nextPos = currentGridPos + moveDir;

    //    Debug.Log($"Current : {currentGridPos}");
    //    Debug.Log($"Move : {moveDir}");
    //    Debug.Log($"Next : {nextPos}");

    //    // 1. 배열 범위 체크
    //    if (nextPos.x < 0 || nextPos.x >= grid.GetLength(0) ||
    //        nextPos.y < 0 || nextPos.y >= grid.GetLength(1)) return null;

    //    Node currNode = grid[currentGridPos.y, currentGridPos.x];
    //    Node nextNode = grid[nextPos.y, nextPos.x];

    //    // 2. 노드가 존재하고 현재 노드와 연결되어 있는지 체크
    //    if (nextNode != null && currNode.nextNode.Contains(nextNode))
    //    {
    //        currentGridPos = nextPos; // 성공 시 논리적 좌표 갱신
    //        return nextNode;
    //    }

    //    return null;
    //}
}
