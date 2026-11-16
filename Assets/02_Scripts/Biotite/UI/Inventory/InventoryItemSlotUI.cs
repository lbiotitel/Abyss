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

    // 안전장치(Null 체크): 데이터가 없으면 0을 반환해서 오류를 막습니다.
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
        // 데이터가 없는 상태에서 회전을 시도하면 무시합니다.
        if (itemObject == null || itemObject.ItemData == null) return;

        // 가로세로 크기가 같으면 회전할 필요가 없으므로 원본 데이터로 비교해서 무시합니다.
        if (itemObject.ItemData.Width == itemObject.ItemData.Height) return;

        rotated = !rotated;
        rectTransform.DOComplete();
        rectTransform.DORotate(new Vector3(0, 0, rotated ? 90f : 0f), 0.1f);
    }

    public void OnOffRayCast(bool onOff) => itemImage.raycastTarget = onOff;



    //마우스 우클릭 시 아이템을 사용합니다. 이때 아이템을 집은 상태면 안 됩니다.
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