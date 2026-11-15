using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterShopUI : MonoBehaviour
{
    [SerializeField] private Ray ray;
    [SerializeField] private RaycastHit hit;

    [Header("UI")]
    [SerializeField] private GameObject uiParent;
    [SerializeField] private GameObject objShopUI;
    [SerializeField] private bool isUIOpened;

    private void Awake()
    {
        uiParent = GameObject.FindWithTag("Shop_Dungeon");
        objShopUI = uiParent.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        Ray click = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            // 9: ShopInteractive ·¹ÀÌ¾î
            if (Physics.Raycast(click, out hit) && hit.transform.gameObject.layer == 9
                && isUIOpened == false)
            {
                isUIOpened = true;
                objShopUI.SetActive(true);
            }
        }

        isUIOpened = false;
    }
}
