using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;


public class EquipmentSlotUI : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private EEquipmentType equipmentType;
    [SerializeField] private Image equipmentImage;
    private EquipmentData equipmentData;
    private RectTransform rectTransform;

    public EEquipmentType EquipmentType => equipmentType;

    public event Action<EquipmentSlotUI, InventoryItemSlotUI> OnClickedEquipmentSlotUI;

    private void Awake()
    {
        equipmentImage.preserveAspect = true;
        rectTransform = GetComponent<RectTransform>();
        UnEquip();
    }


    //3가지 경우를 고려해야함
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Managers.Battle.currentState == EBattleState.Battle) return;

        InventoryItemSlotUI inventoryItemSlotUI = MouseObject.Instance.SelectedItem;
        OnClickedEquipmentSlotUI?.Invoke(this, inventoryItemSlotUI);
    }


    //여기는 ui만 그리고 실제 로직은 equipment system에서 내부적으로 처리
    public void Equip(EquipmentData equipmentData)
    {
        this.equipmentData = equipmentData;
        equipmentImage.gameObject.SetActive(true);
        equipmentImage.sprite = equipmentData.ItemIcon;
    }

    public void UnEquip()
    {
        equipmentData = null;
        equipmentImage.sprite = null;
        equipmentImage.gameObject.SetActive(false);
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        if (equipmentData == null) return;
        Managers.Event.NotifyEvent<OnItemSlotMouseEntered>(new OnItemSlotMouseEntered
        {
            itemData = equipmentData,
            rectTransform = rectTransform
        });
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        Managers.Event.NotifyEvent<OnItemSlotMouseExited>(new OnItemSlotMouseExited { });
    }


}
