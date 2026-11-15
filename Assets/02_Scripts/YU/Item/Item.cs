using UnityEngine;

public class Item : MonoBehaviour
{
    private ItemDataBase itemData;

    public ItemDataBase ItemData => itemData;


    public void Init(ItemDataBase itemData)
    {
        this.itemData = itemData;
    }
}
