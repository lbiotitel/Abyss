using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHighLight : MonoBehaviour
{
    [SerializeField] RectTransform highLighter;


    private void Start()
    {
        Hide();
    }

    public void SetSize(InventoryItemSlotUI targetItem)
    {
        Vector2 size = new Vector2();
        size.x = targetItem.Width * InventorySystem.tileWidth;
        size.y = targetItem.Height * InventorySystem.tileHeight;
        highLighter.sizeDelta = size;
    }

    public void Show() => highLighter.gameObject.SetActive(true);
    public void Hide() => highLighter.gameObject.SetActive(false);
}
