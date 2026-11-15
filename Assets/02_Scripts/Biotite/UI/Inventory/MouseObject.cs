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



    //�������� ȸ����ŵ�ϴ�.
    public void RotateItem()
    {
        if (SelectedItem == null) return;
        if (Input.GetMouseButtonDown(1))
        {
            SelectedItem.Rotate();
            inventoryHighLight.SetSize(SelectedItem);
        }
    }



    //���콺 ������Ʈ�� �������� �Ҵ��մϴ�.
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
   
    //�׸��� �󿡼� �������� Ŭ������ ��
    private void ItemClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (SelectedItemGrid == null) return;

            Vector2Int gridPos = GetTileGridPosition();

            if (SelectedItem == null)
            {
                //�������� ���������ʰ� �׳� �Ⱦ�
                SelectedItem = SelectedItemGrid.PickUpItem(gridPos);

                if (SelectedItem != null)
                {
                    AssignItem(SelectedItem);
                    Debug.Log($"{SelectedItem.name} Ŭ�� ��");
                }
            }
            else
            {
                //�������������� ��ġ�� ������Ʈ�� �ִٸ� �� ������Ʈ�� ��ġ�� �ٲٰ� ��ġġ ������ �� ��ġ�� ����
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


    //���� Ŭ���� �׸��� ��ǥ�� ������
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



    //��������ġ �ǽð� ����
    private void ItemDrag()
        => rectTransform.position = Input.mousePosition;



    //���õ� �������� Ŭ���� �մϴ�.
    public void ClearItem()
    {
        Destroy(SelectedItem.gameObject);
        inventoryHighLight.Hide();
        SelectedItem = null;
    }
}
