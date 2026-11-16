using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



//스킬 슬롯 ui 입니다
public class SkillNodUI : MonoBehaviour
{
    [SerializeField] private Image skillIconBG;
    [SerializeField] private Image skillIcon;

    public void Init(Sprite skillIcon)
    {
        skillIconBG.sprite = skillIcon;
        this.skillIcon.sprite = skillIcon;
    }

    public void CoolTime(float time)
    {
        AsyncCoolTime(time).Forget();
    }

    private async UniTaskVoid AsyncCoolTime(float time)
    {
        float t = 0;
        skillIcon.fillAmount = 0f;

        var cancelToken = this.GetCancellationTokenOnDestroy();

        while (t < time)
        {
            skillIcon.fillAmount = t / time;
            t += Time.deltaTime;
            await UniTask.Yield(PlayerLoopTiming.Update, cancelToken);
        }

        skillIcon.fillAmount = 1f;
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
}