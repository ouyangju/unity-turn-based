using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillInfoDisplay : MonoBehaviour
{
    public GameObject skillInfoPanel; // 技能信息面板
    public Text skillInfoText; // 显示技能信息的文本组件

    public SkillData data;

    public void Awake()
    {
        skillInfoPanel.SetActive(false);
    }

    // 显示技能信息面板
    public void ShowSkillInfo(SkillData skillData)
    {
        data = skillData;
        data.skillName = skillData.skillName;
        data.skillDescription = skillData.skillDescription;
        skillInfoPanel.SetActive(true);
        skillInfoText.text = "技能名称：" + data.skillName + "\n" + "技能描述：" + data.skillDescription;
    }

    // 隐藏技能信息面板
    public void HideSkillInfo()
    {
        skillInfoPanel.SetActive(false);
    }

    // 当鼠标进入技能UI时调用
    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowSkillInfo(data);
    }

    // 当鼠标退出技能UI时调用
    public void OnPointerExit(PointerEventData eventData)
    {
        HideSkillInfo();
    }
}
