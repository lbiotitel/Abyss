using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



//상점에 물건을 등록하는 노드 입니다.
public class ShopSlotUI : MonoBehaviour,IPointerClickHandler
{

    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text priceText;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float sellingAlha = 0.7f;
    [SerializeField] private float originAlpha = 1.0f;

    private ItemDataBase itemdata;
    private bool isSelling = false;

    public event Action<ShopSlotUI,ItemDataBase> OnClicked;



    public void Init(ItemDataBase itemData)
    { 
        this.itemdata = itemData;
        canvasGroup.alpha = originAlpha;
        itemImage.color = Color.white;
        isSelling = false;
        DrawNodeUI();
    }

    private void DrawNodeUI()
    {
        itemImage.sprite = itemdata.ItemIcon;
        itemNameText.text = itemdata.ItemName;
        priceText.text = $"{itemdata.ItemPrice}G";
    }


    //클릭시 이벤트를 알려 실제 로직은 Shop에서 처리하도록 함
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isSelling) return;
        OnClicked?.Invoke(this,itemdata);
    }

    public void Sell()
    {
        isSelling = true;
        itemImage.color = Color.gray;
        canvasGroup.alpha = sellingAlha;
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);

}
