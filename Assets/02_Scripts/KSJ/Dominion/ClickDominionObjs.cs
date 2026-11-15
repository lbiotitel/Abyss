using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDominionObjs : MonoBehaviour
{
    [SerializeField] private Ray ray;
    [SerializeField] private RaycastHit hit;

    [Header("UI")]
    [SerializeField] private GameObject objDominionUI;

    [Header("선택된 건물 오브젝트")]
    [SerializeField] private GameObject selectedObj;

    [Header("카메라")]
    [SerializeField] private GameObject cam;
    [SerializeField] private Transform defaultPosition;
    [SerializeField] private Vector3 cameraCenter;

    public Action<PlaceInformation> OnUIChanged;

    private void Awake()
    {
        cam.transform.position = new Vector3(defaultPosition.position.x, defaultPosition.position.y, defaultPosition.position.z);
        cameraCenter = new Vector3(Screen.width / 2, Screen.height / 2, defaultPosition.position.z);
    }

    private void Update()
    {
        Ray click = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonUp(0))
        {
            // 6: DominionObj 레이어
            if (Physics.Raycast(click, out hit) && hit.transform.gameObject.layer == 6)
            {
                objDominionUI.SetActive(true);
                OnUIChanged?.Invoke(hit.transform.gameObject.GetComponent<PlaceInformation>());
            }
        }
    }

}
