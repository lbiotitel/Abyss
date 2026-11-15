using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour,IPoolingObject
{
    [SerializeField] private Vector3 anim;
    [SerializeField] private float animDuration;
    [SerializeField] private TMP_Text damageText;

    public GameObject OriginPrefab { get; set; }

    public void Init(string msg, EDamageTextType damageTextType)
    {
        damageText.text = msg;
        SetColor(damageTextType);
        DoMotion();
    }

    public void ResetObject()
    {
        transform.DOKill();
        damageText.text = "";
        damageText.color = Color.white;
    }

    public void ReturnObject()
    {
        ResetObject();
        Managers.Object.PoolingSystem.ReturnObject(gameObject);
    }

    private void DoMotion()
    {
        Vector3 targetPos = transform.position + anim;
        transform.DOMove(targetPos, animDuration);
        damageText.DOFade(0, animDuration)
            .OnComplete(() => ReturnObject());
    }

    private void SetColor(EDamageTextType damageTextType)
    {
        switch (damageTextType)
        {
            case EDamageTextType.NORMAL:
                damageText.color = Color.white;
                break;
            case EDamageTextType.CRITICAL:
                damageText.color = Color.red;
                break;
            case EDamageTextType.POISION:
                damageText.color = Color.green;
                break;
            case EDamageTextType.IGNITE:
                damageText.color = new Color(1f, 0.5f, 0f);
                break;
            case EDamageTextType.BLEED:
                damageText.color = new Color(0.5f, 0f, 0f);
                break;
            case EDamageTextType.FROZEN:
                damageText.color = Color.blue;
                break;
        }
    }
}


public enum EDamageTextType
{
    NORMAL,
    CRITICAL,
    POISION,
    IGNITE,
    FROZEN,
    BLEED
}