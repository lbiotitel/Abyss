using UnityEngine;
using UnityEngine.EventSystems;

public class MouseObject : Singleton<MouseObject>
{
    public InventorySystem SelectedItemGrid { get; set; } = null;
    public InventoryItemSlotUI SelectedItem { get; private set; } = null;

    private InventoryHighLight inventoryHighLight;
    private RectTransform rectTransform;


    protected override void Awake()
    {
        base.Awake();
        rectTransform = GetComponent<RectTransform>();
        inventoryHighLight = GetComponent<InventoryHighLight>();
    }


    private void Update()
    {
        ItemDrag();
        RotateItem();
        ItemClicked();
        DropItem();
    }



    //아이템을 회전시킵니다.
    public void RotateItem()
    {
        if (SelectedItem == null) return;
        if (Input.GetMouseButtonDown(1))
        {
            SelectedItem.Rotate();
            inventoryHighLight.SetSize(SelectedItem);
        }
    }



    //마우스 오브젝트에 아이템을 할당합니다.
    public void AssignItem(InventoryItemSlotUI inventoryItem)
    {
        SelectedItem = inventoryItem;
        SelectedItem.transform.SetParent(transform);
        SelectedItem.OnOffRayCast(false);
        SelectedItem.transform.localPosition = Vector3.zero;

        inventoryHighLight.SetSize(inventoryItem);
        inventoryHighLight.Show();

        Managers.SoundManager.PlaySFX(KeyDefines.AudioKey.EQUIP_SFX);
    }


    //아이템을 버립니다 
    private void DropItem()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (!Managers.Game.InGame.isGameStart) return;
            if (Managers.Battle.currentState == EBattleState.Battle) return;
            if (SelectedItem == null) return;

            inventoryHighLight.Hide();
            Destroy(SelectedItem.gameObject);
            SelectedItem = null;
        }
    }
   
    //그리드 위에서 아이템을 클릭했을 때
    private void ItemClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (SelectedItemGrid == null) return;

            Vector2Int gridPos = GetTileGridPosition();

            if (SelectedItem == null)
            {
                //들고 있는 아이템이 없으면 클릭한 아이템을 집음
                SelectedItem = SelectedItemGrid.PickUpItem(gridPos);

                if (SelectedItem != null)
                {
                    AssignItem(SelectedItem);
                    Debug.Log($"{SelectedItem.name} 클릭 됨");
                }
            }
            else
            {
                //들고 있는 아이템을 놓을 자리에 다른 아이템이 있다면 서로 위치를 바꿈
                InventoryItemSlotUI overlapItem = null;
                if (SelectedItemGrid.PlaceItem(SelectedItem, gridPos, ref overlapItem))
                {
                    inventoryHighLight.Hide();
                    SelectedItem.OnOffRayCast(true);
                    SelectedItem = null;
                    Managers.SoundManager.PlaySFX(KeyDefines.AudioKey.EQUIP_SFX);
                    if (overlapItem != null)
                    {
                        AssignItem(overlapItem);
                        overlapItem = null;
                    }
                }
            }
        }
    }


    //현재 마우스가 가리키는 그리드 좌표를 가져옴
    private Vector2Int GetTileGridPosition()
    {
        Vector2 position = Input.mousePosition;

        if (SelectedItem != null)
        {
            position.x -= ((SelectedItem.Width - 1) * InventorySystem.tileWidth / 2f);
            position.y += ((SelectedItem.Height - 1) * InventorySystem.tileHeight / 2f);
        }

        return SelectedItemGrid.GetGridPosition(position);
    }



    //들고 있는 아이템 위치를 마우스에 맞춰 실시간 갱신
    private void ItemDrag()
        => rectTransform.position = Input.mousePosition;



    //선택된 아이템을 제거합니다.
    public void ClearItem()
    {
        Destroy(SelectedItem.gameObject);
        inventoryHighLight.Hide();
        SelectedItem = null;
    }
}
