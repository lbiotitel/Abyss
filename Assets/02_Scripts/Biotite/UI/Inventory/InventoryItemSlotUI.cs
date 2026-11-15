using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemSlotUI : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public ItemObject itemObject { get; private set; }
    public Vector2Int OriginPos { get; set; }
    private RectTransform rectTransform;
    private Image itemImage;
    private bool rotated = false;

    // ������ġ(Null üũ) �߰�: �����Ͱ� ���� ���� 0�� ��ȯ�Ͽ� ���� ȣ�� ����
    public int Width
    {
        get
        {
            if (itemObject == null || itemObject.ItemData == null) return 0;
            return rotated ? itemObject.ItemData.Height : itemObject.ItemData.Width;
        }
    }

    public int Height
    {
        get
        {
            if (itemObject == null || itemObject.ItemData == null) return 0;
            return rotated ? itemObject.ItemData.Width : itemObject.ItemData.Height;
        }
    }

    private void Awake()
    {
        itemImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void Init(ItemObject inventoryItem)
    {
        this.itemObject = inventoryItem;
        SetItemIconSize();
    }

    private void SetItemIconSize()
    {
        if (itemObject == null || itemObject.ItemData == null) return;

        Vector2 size = new Vector2();
        size.x = itemObject.ItemData.Width * InventorySystem.tileWidth;
        size.y = itemObject.ItemData.Height * InventorySystem.tileHeight;

        itemImage.sprite = itemObject.ItemData.ItemIcon;
        rectTransform.sizeDelta = size;
    }

    public void Rotate()
    {
        // �����Ͱ� ���� ���¿��� ȸ���� �õ��ϸ� ����
        if (itemObject == null || itemObject.ItemData == null) return;

        // ������Ƽ getter ��� ���� �����͸� ���� ���Ͽ� ���� ������Ƽ ȣ�� ����
        if (itemObject.ItemData.Width == itemObject.ItemData.Height) return;

        rotated = !rotated;
        rectTransform.DOComplete();
        rectTransform.DORotate(new Vector3(0, 0, rotated ? 90f : 0f), 0.1f);
    }

    public void OnOffRayCast(bool onOff) => itemImage.raycastTarget = onOff;



    //���콺 ������ Ŭ���� �������� ����մϴ�. �̶� ���콺�� ������ �ȵ�
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (!Managers.Game.InGame.isGameStart) return;
            itemObject.UseItem();
        }
    }


    //마우스 호버시 툴팁을 나타냅니다.
    public void OnPointerEnter(PointerEventData eventData)
    {
        Managers.Event.NotifyEvent<OnItemSlotMouseEntered>(new OnItemSlotMouseEntered
        {
            itemData = itemObject.ItemData,
            rectTransform = rectTransform
        });
    }

    //툴팁을 숨깁니다.
    public void OnPointerExit(PointerEventData eventData)
    {
         Managers.Event.NotifyEvent<OnItemSlotMouseExited>(new OnItemSlotMouseExited{});
    }
}