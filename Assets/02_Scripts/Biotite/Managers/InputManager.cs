using Unity.VisualScripting;
using UnityEngine;
// UI 및 이벤트 시스템 체크를 위해 추가 필요
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public bool StatButton { get; private set; } = false;
    public bool InteractButton { get; private set; } = false;
    public bool PlayerStatButton { get; private set; } = false;
    public Vector2 MoveInput { get; private set; } = Vector2.zero;
    public Vector2 MousePosition { get; private set; } = Vector2.zero;
    public bool AttackButton { get; private set; } = false;
    public bool ParryingButton { get; private set; } = false;

    private void Update()
    {
        CheckMoveInput();
        CheckMousePosition();
        CheckInteractButton();
        CheckPlayerStatButton();
        CheckReloadButton();
        CheckAttackButton();
        CheckParryingButton();
    }

    private void CheckAttackButton()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            AttackButton = true;
        }
        else
        {
            AttackButton = false;
        }
    }

    private void CheckParryingButton()
    {
        if (Input.GetMouseButton(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            ParryingButton = true;
        }
        else
        {
            ParryingButton = false;
        }
    }

    private void CheckMoveInput()
        => MoveInput = new Vector2(0, Input.GetAxisRaw("Vertical")).normalized;

    private void CheckMousePosition()
        => MousePosition = Input.mousePosition;

    private void CheckPlayerStatButton()
        => PlayerStatButton = Input.GetKeyDown(KeyCode.C);

    private void CheckInteractButton()
        => InteractButton = Input.GetKey(KeyCode.E);

    private void CheckReloadButton()
        => StatButton = Input.GetKeyDown(KeyCode.Tab);
}