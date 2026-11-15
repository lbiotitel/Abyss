using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceChecker : MonoBehaviour
{
    [SerializeField] MonoBehaviour script;
    private void OnEnable()
    {
        Managers.Resource.IsDataFullyLoaded += InitSystem;
        script = gameObject.GetComponent<MonoBehaviour>();
    }

    private void InitSystem()
    {
        if (script != null) script.enabled = true;
    }

    private void OnDisable()
    {
        Managers.Resource.IsDataFullyLoaded -= InitSystem;
    }
}
