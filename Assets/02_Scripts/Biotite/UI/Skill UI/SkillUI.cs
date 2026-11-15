using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private Transform gridTransform;
    [SerializeField] private SkillNodUI skillNodeUIPrefab;
    [SerializeField] private int poolingCount = 13;

    private Dictionary<SkillBase, SkillNodUI> skillNodeUIDict;
    private Queue<SkillNodUI> skillNodUIs;

    public void Init()
    {
        skillNodeUIDict = new Dictionary<SkillBase, SkillNodUI>();    
        skillNodUIs = new Queue<SkillNodUI>();
        InitNodeUIs();
    }


    //NODE UI를 미리 풀링 하여 저장합니다.
    private void InitNodeUIs()
    { 
        for (int i = 0; i < poolingCount; i++)
        {
            SkillNodUI nodeUI = Instantiate(skillNodeUIPrefab);
            skillNodUIs.Enqueue(nodeUI);
            nodeUI.transform.SetParent(gridTransform);
            nodeUI.Hide();
        }
    }


    //스킬을 ui에 등록합니다.
    public void AddSkillUI(SkillBase skill)
    {
        SkillNodUI nodeUI = skillNodUIs.Dequeue();
        nodeUI.Init(skill.SkillData.SkillIcon);
        nodeUI.Show();
        skillNodeUIDict.Add(skill, nodeUI);
    }


    //스킬을 ui에서 제거합니다.
    public void RemoveSkillUI(SkillBase skill)
    {
        if (skillNodeUIDict.TryGetValue(skill, out SkillNodUI nodeUI))
        {
            nodeUI.Hide();
            skillNodUIs.Enqueue(nodeUI);
            skillNodeUIDict.Remove(skill);
        }
    }


    public void CoolTime(SkillBase targetSkill, float t)
    {
        if (skillNodeUIDict.TryGetValue(targetSkill, out SkillNodUI nodeUI))
        {
            nodeUI.CoolTime(t);
        }
    }



    public void HideAllUIs()
    {
        foreach (var node in skillNodUIs)
        {
            node.gameObject.SetActive(false);
        }
    }
}
