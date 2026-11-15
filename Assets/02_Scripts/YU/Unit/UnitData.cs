using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/EnemyData")]
public class UnitData : ScriptableObject
{
    [SerializeField] private string unitName;
    [SerializeField] private AnimatorController animatorController;
    //[SerializeField] private Mesh mesh;
    //[SerializeField] private Material materal;
    [SerializeField] private GameObject model;

    [SerializeField] private List<SStatContainer> statContainers;

    public string UnitName => unitName;
    public AnimatorController AnimatorController => animatorController;
    //public Mesh Mesh => mesh;
    //public Material Material => materal;
    public GameObject Model => model;


    public List<SStatContainer> StatContainers => statContainers;
}
