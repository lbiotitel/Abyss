using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Open();

            Vector3 distance = other.transform.position - transform.position;
            
            int direction = distance.z >= 0 ? -1 : 1;

            // 현재 방의 상태를 알리고 변경하기
            Managers.Event.NotifyEvent<OnNextRoom>(new OnNextRoom { moveDir = direction });
            return;
        }
    }


    private void Open()
    {
        animator.SetTrigger("Open");
        Managers.SoundManager.PlaySFX(KeyDefines.AudioKey.OPEN_GATE_SFX);
    }
}
