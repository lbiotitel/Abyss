using UnityEngine;

[CreateAssetMenu(menuName = "Data/PlayerData")]
public class PlayerData : UnitData
{
    [SerializeField] private Transform swapPos;

    public Transform SwapPos => swapPos;
}
