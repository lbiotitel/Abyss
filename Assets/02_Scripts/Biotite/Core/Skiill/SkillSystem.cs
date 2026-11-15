using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;



//��ų�� �����ϴ� Ŭ���� �Դϴ�.
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



    //��ų ���
    public void AddSkill(SkillData skillData)
    {
        SkillBase skill = new SkillBase();
        skill.Init(player,skillData);
        skills.Add(skill);
        skillUI.AddSkillUI(skill);
        skill.OnSkillActive += HandleSkillUseEvent;
        Logger.Log($"{skillData.SkillName} ��ų�� ��ϵǾ����ϴ�.");
    }



    //��ϵ� ��ų ����
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
                Logger.Log($"{skillData.SkillName} ��ų�� �����Ǿ����ϴ�.");
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



    //��ų�� ������ ���� �̺�Ʈ�� �ٷ�ϴ�.
    public void HandleSkillUseEvent(SkillBase skill, float coolTime)
    {
        skillUI.CoolTime(skill, coolTime);
    }

}
