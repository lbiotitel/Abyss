using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUtils : MonoBehaviour
{

    public class Grid
    {


        //그리드 상의 좌표를 계산해서 돌려줍니다
        public static Vector2 CalculatePositionOnGrid(InventoryItemSlotUI inventoryItem, Vector2Int pos)
        {
            Vector2 position = new Vector2();
            int tileWidth = InventorySystem.tileWidth;
            int tileHeight = InventorySystem.tileHeight;

            position.x = pos.x * tileWidth + (tileWidth * inventoryItem.Width / 2f);
            position.y = -(pos.y * tileHeight + (tileHeight * inventoryItem.Height / 2f));
            return position;
        }
    }
}
