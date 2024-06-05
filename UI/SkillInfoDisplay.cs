using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillInfoDisplay : MonoBehaviour
{
    public GameObject skillInfoPanel; // ������Ϣ���
    public Text skillInfoText; // ��ʾ������Ϣ���ı����

    public SkillData data;

    public void Awake()
    {
        skillInfoPanel.SetActive(false);
    }

    // ��ʾ������Ϣ���
    public void ShowSkillInfo(SkillData skillData)
    {
        data = skillData;
        data.skillName = skillData.skillName;
        data.skillDescription = skillData.skillDescription;
        skillInfoPanel.SetActive(true);
        skillInfoText.text = "�������ƣ�" + data.skillName + "\n" + "����������" + data.skillDescription;
    }

    // ���ؼ�����Ϣ���
    public void HideSkillInfo()
    {
        skillInfoPanel.SetActive(false);
    }

    // �������뼼��UIʱ����
    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowSkillInfo(data);
    }

    // ������˳�����UIʱ����
    public void OnPointerExit(PointerEventData eventData)
    {
        HideSkillInfo();
    }
}
