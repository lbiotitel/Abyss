using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//플레이어의 스킬을 등록하고 관리하는 클래스입니다.
public class SkillSystem : MonoBehaviour
{
    
    private Player player;
    private List<SkillBase> skills;
    private SkillUI skillUI;

    public void Init(Player player)
    { 
        this.player = player;
        skills = new List<SkillBase>();
        skillUI = FindAnyObjectByType<SkillUI>();
        skillUI.Init();
    }



    //스킬 등록
    public void AddSkill(SkillData skillData)
    {
        SkillBase skill = new SkillBase();
        skill.Init(player,skillData);
        skills.Add(skill);
        skillUI.AddSkillUI(skill);
        skill.OnSkillActive += HandleSkillUseEvent;
        Logger.Log($"{skillData.SkillName} 스킬이 등록되었습니다.");
    }



    //등록된 스킬 제거
    public void RemoveSkill(SkillData skillData) 
    {
        for (int i = skills.Count - 1; i >= 0; i--)
        {
            if (skillData == skills[i].SkillData)
            {
                skills[i].OnSkillActive -= HandleSkillUseEvent;
                skillUI.RemoveSkillUI(skills[i]);
                skills[i].Release();
                skills.RemoveAt(i);
                Logger.Log($"{skillData.SkillName} 스킬이 제거되었습니다.");
            }
        }
    }


    private void OnDestroy()
    {
        for (int i = skills.Count - 1; i >= 0; i--)
        {
            skills[i].OnSkillActive -= HandleSkillUseEvent;
            skills[i].Release();
            skills.RemoveAt(i);
        }
    }



    //스킬이 발동되면 쿨타임 UI를 갱신합니다.
    public void HandleSkillUseEvent(SkillBase skill, float coolTime)
    {
        skillUI.CoolTime(skill, coolTime);
    }

}
