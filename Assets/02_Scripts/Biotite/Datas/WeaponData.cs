using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu]
public class WeaponData : EquipmentData
{
    [SerializeField] private GameObject weaponModel;
    [SerializeField] private Vector3 grabPos;
    [SerializeField] private Vector3 parryingPos;

    [Header("Motion")]
    [SerializeField] private WeaponMotionData weaponMotionData;
    [SerializeField] private string weaponSFX;

    public WeaponMotionData WeaponMotionData => weaponMotionData;
    public GameObject WeaponModel => weaponModel;
    public Vector3 GrabPos => grabPos;
    public Vector3 ParryingPos => parryingPos;
    public string WeaponSFX => weaponSFX;
}



