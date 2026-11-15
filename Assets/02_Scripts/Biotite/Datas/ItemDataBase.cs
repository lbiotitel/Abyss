using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu]
public class ItemDataBase : ScriptableObject
{
    [Header("Properties")]
    [SerializeField] protected string itemID;
    [SerializeField] protected Sprite itemIcon;
    [SerializeField] protected string itemName;
    [SerializeField, TextArea] protected string itemDesc;
    [SerializeField] protected EItemType itemType;
    [SerializeField] protected ERarity rarity;
    [SerializeField] protected int itemPrice;
    [SerializeField] protected int ironStone;
    [SerializeField] protected int wood;
    [SerializeField] private bool inShop = true;

    [Header("Grid")]
    [SerializeField] protected int height;
    [SerializeField] protected int width;


    private void OnValidate()
    {
        itemID = name;
        height = Mathf.Clamp(height,1,4);
        width = Mathf.Clamp(width,1,4);
    }


    public bool InShop => inShop;
    public string ItemID => itemID;
    public Sprite ItemIcon => itemIcon;
    public string ItemName  => itemName;
    public string ItemDesc => itemDesc;
    public EItemType ItemType => itemType;
    public ERarity Rarity => rarity;
    public int Height => height;
    public int Width => width;
    public int ItemPrice => itemPrice;
    public int IronStone => ironStone;  
    public int Wood => wood;    
}

public enum EItemType
{
    Equipment,
    Consumable,
    Resource
}

public enum ERarity
{ 
    Normal,
    Rare,
    Unique,
    Legend
}