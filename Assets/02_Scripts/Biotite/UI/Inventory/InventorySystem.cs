using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



//인벤토리 시스템을 정의 합니다.
public class InventorySystem : MonoBehaviour
{
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;

    public const int tileWidth = 76;
    public const int tileHeight = 76;

    private RectTransform rectTransform;
    private InventoryItemSlotUI[,] inventoryItemSlot;
    private bool isInitialized = false;

    //인벤토리 시스템 사용시 초기화 필수
    public void Init()
    {
        isInitialized = true;
        rectTransform = GetComponent<RectTransform>();
        inventoryItemSlot = new InventoryItemSlotUI[gridHeight, gridWidth];
        Vector2 size = new Vector2(gridWidth * tileWidth, gridHeight * tileHeight);
        rectTransform.sizeDelta = size;
    }


    private bool CheckInit()
    {
        if (!isInitialized)
        {
            Logger.Log("인벤토리가 초기화 되지않았습니다.");
            return false;
        }
        return true;
    }


    //인벤토리의 남은 공간을 찾아 아이템을 추가합니다.
    public void AddToInventory(InventoryItemSlotUI newItem)
    {
        if (!CheckInit()) return;

        Vector2Int pos = FindSpace(newItem);

        if (pos.x == -1)
        {
            Destroy(newItem.gameObject);
            return;
        }

        PlaceItem(newItem, pos);
    }


    // 앞서 교정한 대로 GetWorldCorners를 이용해 정확한 좌상단 기준 타일 좌표 반환
    public Vector2Int GetGridPosition(Vector2 targetPos)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        Vector2 leftTopCorner = corners[1];

        Vector2 rectPos = new Vector2();
        rectPos.x = targetPos.x - leftTopCorner.x;
        rectPos.y = leftTopCorner.y - targetPos.y;

        Vector2Int gridPos = new Vector2Int();
        gridPos.x = Mathf.FloorToInt(rectPos.x / tileWidth);
        gridPos.y = Mathf.FloorToInt(rectPos.y / tileHeight);

        return gridPos;
    }


    public bool PlaceItem(InventoryItemSlotUI inventoryItem, Vector2Int pos, ref InventoryItemSlotUI overlapItem)
    {
        if (!CheckInit()) return false;

        if (!BoundaryCheck(pos, inventoryItem.Width, inventoryItem.Height)) return false;

        if (!OverlapCheck(inventoryItem, pos, ref overlapItem))
        {
            overlapItem = null;
            return false;
        }

        if (overlapItem != null)
        {
            SetInventoryItemSlot(overlapItem, overlapItem.OriginPos, true);
        }

        return PlaceItem(inventoryItem, pos);
    }

    public bool PlaceItem(InventoryItemSlotUI inventoryItem, Vector2Int pos)
    {
        if (!BoundaryCheck(pos, inventoryItem.Width, inventoryItem.Height)) return false;

        RectTransform itemRect = inventoryItem.GetComponent<RectTransform>();
        itemRect.SetParent(rectTransform);

        SetInventoryItemSlot(inventoryItem, pos);

        inventoryItem.OriginPos = pos;

        Vector2 position = InventoryUtils.Grid.CalculatePositionOnGrid(inventoryItem, pos);
        itemRect.localPosition = position;

        return true;
    }


    //특정 배열에 아이템 정보를 설정합니다. clear가 true이면 해당 위치를 null로 초기화합니다.
    private void SetInventoryItemSlot(InventoryItemSlotUI inventoryItem, Vector2Int pos, bool clear = false)
    {
        if (!CheckInit()) return;
        int width = inventoryItem.Width;
        int height = inventoryItem.Height;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                inventoryItemSlot[pos.y + y, pos.x + x] = clear ? null : inventoryItem;
            }
        }
    }


    //현재 그리드에 위치한 아이템의 참조를 반환합니다.
    public InventoryItemSlotUI PickUpItem(Vector2Int gridPos)
    {
        if (!CheckInit()) return null;
        if (!CheckRange(gridPos)) return null;

        InventoryItemSlotUI res = inventoryItemSlot[gridPos.y, gridPos.x];
        if (res == null) return null;

        Vector2Int originPos = res.OriginPos;
        SetInventoryItemSlot(res, originPos, true);

        return res;
    }

    //아이템이 겹쳐지는지 체크하고 겹쳐지면 겹쳐진 아이템의 참조를 반환합니다.
    public bool OverlapCheck(InventoryItemSlotUI inventoryItem, Vector2Int pos, ref InventoryItemSlotUI overlapItem)
    {
        if (!CheckInit()) return false;
        int width = inventoryItem.Width;
        int height = inventoryItem.Height;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                InventoryItemSlotUI slotItem = inventoryItemSlot[pos.y + i, pos.x + j];

                if (slotItem != null)
                {
                    if (overlapItem == null)
                    {
                        overlapItem = slotItem;
                    }
                    else if (overlapItem != slotItem)
                    {
                        return false;
                    }
                }
            }
        }

        return true; 
    }
    



    //현재 위치가 그리드 좌표위치상에 있는지 체크합니다.
    public bool CheckRange(Vector2Int pos)
    {
        if (pos.x < 0 || pos.y < 0 || pos.x >= gridWidth || pos.y >= gridHeight) return false;
        return true;
    }

    //아이템의 크기가 격자에 알맞은지 체크합니다.
    public bool BoundaryCheck(Vector2Int pos, int width, int height)
    {
        if (!CheckRange(pos)) return false;

        Vector2Int endPos = new Vector2Int(pos.x + width - 1, pos.y + height - 1);
        if (!CheckRange(endPos)) return false;

        return true;
    }

    //인벤토리에서 남은 부분을 찾아 공간을 리턴합니다.
    public Vector2Int FindSpace(InventoryItemSlotUI inventoryItem)
    {
        if (!CheckInit()) return new Vector2Int(-1,-1);
        int width = inventoryItem.Width;
        int height = inventoryItem.Height;

        for (int y = 0; y <= gridHeight - height; y++)
        {
            for (int x = 0; x <= gridWidth - width; x++)
            {
                Vector2Int gridPos = new Vector2Int(x, y);
                if (CheckRemainSpace(inventoryItem, gridPos)) return gridPos;
            }
        }
        return new Vector2Int(-1, -1);
    }


    //남은 공간이 있는 지 체크합니다.
    public bool CheckRemainSpace(InventoryItemSlotUI inventoryItem, Vector2Int pos)
    {
        if (!CheckInit()) return false;
        int width = inventoryItem.Width;
        int height = inventoryItem.Height;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (inventoryItemSlot[pos.y + i, pos.x + j] != null) return false;
            }
        }
        return true;
    }



    //현재 인벤토리에 있는 모든 아이템 정보를 가지고 옵니다.
    public List<ItemDataBase> GetAllInventoryItemData()
    {
        if (!CheckInit()) return null;

        List<ItemDataBase> res = new List<ItemDataBase>();
        HashSet<ItemDataBase> h = new HashSet<ItemDataBase>();

        for (int i = 0; i < gridHeight; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                if (inventoryItemSlot[i,j] != null)
                {
                    h.Add(inventoryItemSlot[i,j].itemObject.ItemData);
                }
            }
        }

        foreach(var d in h)res.Add(d);
        
        return res;
    }



    //모든 아이템을 인벤토리에서 제거합니다.
    public void ClearItems()
    {
        if (!CheckInit()) return;

        HashSet<InventoryItemSlotUI> targetList = new HashSet<InventoryItemSlotUI>();

        for (int i = 0; i < gridHeight; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                if (inventoryItemSlot[i, j] != null)
                {
                    targetList.Add(inventoryItemSlot[i, j]);
                }
            }
        }

        foreach (var target in targetList)
        { 
            Destroy(target.gameObject); 
        }

        for (int i = 0; i < gridHeight; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                inventoryItemSlot[i, j] = null;
            }
        }
    }
}