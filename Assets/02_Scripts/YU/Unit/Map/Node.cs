using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector2Int pos;
    public ENodeType nodeType;      // 노드 타입

    public List<Node> nextNode;    // 연결될 노드
    public GameObject mapObject;

    public enum ENodeType
    {
        None,
        Start,
        Monster,
        Treasure,
        Event,
        Exit,
    }
}
