using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


//현재 어떤 그리드에 마우스가 가있는지 체크하고 그리드를 바꿉니다
public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private MouseObject mouseObject;
    private InventorySystem itemGrid;

    private void Awake()
    {
        itemGrid = GetComponent<InventorySystem>();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseObject.Instance.SelectedItemGrid = itemGrid;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseObject.Instance.SelectedItemGrid = null;
    }

}
