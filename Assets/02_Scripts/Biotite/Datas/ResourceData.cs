using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class ResourceData : ItemDataBase
{
    private void OnValidate()
    {
        itemType = EItemType.Resource; 
    }
}
