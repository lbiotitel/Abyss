using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;




//�����۵��� ������ �����մϴ�.
public class ToolTipUI : MonoBehaviour
{
    [SerializeField] private RectTransform tooltipRect;
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private Canvas canvas;

    [SerializeField] private Transform contentTransform;

    [Header("Item Common Data")]
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemType;
    [SerializeField] private TMP_Text itemRarity;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescText;

    [Header("Skill Data")]
    [SerializeField] private float skillImageSize;
    [SerializeField] private GameObject equipmentSkillContainer;
    [SerializeField] private Transform equipmentSkillContent;
    [SerializeField] private RectTransform equipmentSkillRect;

    private void Start()
    {
        itemImage.preserveAspect = true;
        Managers.Event.AddEvent<OnItemSlotMouseEntered>(Show);
        Managers.Event.AddEvent<OnItemSlotMouseExited>(Hide);
        Hide(new OnItemSlotMouseExited { });
    }

    private void OnDestroy()
    {
        Managers.Event.RemoveEvent<OnItemSlotMouseEntered>(Show);
        Managers.Event.RemoveEvent<OnItemSlotMouseExited>(Hide);
    }

    private void Show(OnItemSlotMouseEntered e)
    {
        SetTooltipPosition(e.rectTransform.position);
        contentTransform.gameObject.SetActive(true);
        ShowItemCommonData(e.itemData);
    }

    private void SetTooltipPosition(Vector2 screenPos)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out Vector2 localPos);

        localPos += new Vector2(20, -20);

        Vector2 size = tooltipRect.rect.size;
        Vector2 canvasSize = canvasRect.rect.size;

        float minX = -canvasSize.x * 0.5f;
        float maxX = canvasSize.x * 0.5f - size.x;

        float minY = -canvasSize.y * 0.5f + size.y;
        float maxY = canvasSize.y * 0.5f;

        localPos.x = Mathf.Clamp(localPos.x, minX, maxX);
        localPos.y = Mathf.Clamp(localPos.y, minY, maxY);

        tooltipRect.anchoredPosition = localPos;
    }


    //�������� ���� �κ��� �׸��ϴ�.
    public void ShowItemCommonData(ItemDataBase itemData)
    {
        itemImage.sprite = itemData.ItemIcon;
        itemNameText.text = itemData.ItemName;
        itemDescText.text = itemData.ItemDesc;
        itemRarity.text = Utils.itemRarityToKor(itemData.Rarity);
        itemRarity.color = Utils.GetRarityColor(itemData.Rarity);
        itemType.text = Utils.ItemTypeToKor(itemData.ItemType);

        if (itemData.ItemType == EItemType.Equipment)
        {
            EquipmentData equipmentData = itemData as EquipmentData;
            ShowSkillData(equipmentData);
        }
        else
        {
            equipmentSkillContainer.SetActive(false);
            LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipRect);
        }
    }


    //��� �ִ� ���� �����͸� �׸��ϴ�.
    public void ShowSkillData(EquipmentData equipmentData)
    {
        List<SkillData> skillDatas = equipmentData.SkillDatas;
        if (skillDatas == null || skillDatas.Count == 0) return;
        equipmentSkillContainer.SetActive(true);

        for (int i = equipmentSkillContent.childCount - 1; i >= 0; i--)
        {
            Transform childObject = equipmentSkillContent.GetChild(i);
            Destroy(childObject.gameObject);
            childObject.SetParent(null);
        }

        foreach (var skill in skillDatas)
        {
            GameObject skillImg = new GameObject(skill.SkillName);
            Image image = skillImg.AddComponent<Image>();
            image.sprite = skill.SkillIcon;
            image.rectTransform.sizeDelta = new Vector2(skillImageSize, skillImageSize);
            image.transform.SetParent(equipmentSkillContent);

            LayoutRebuilder.ForceRebuildLayoutImmediate(equipmentSkillRect);
            LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipRect);
        }
    }



    public void Hide(OnItemSlotMouseExited e)
    {
        contentTransform.gameObject.SetActive(false);
        equipmentSkillContainer.SetActive(false);
    }
}