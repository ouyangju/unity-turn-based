using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��Ϸ�о���ʹ��ʱ�ļ�������
public class SkillInfo : TurnEvent
{
    public SkillData skillData;
    public GameObject user;
    public GameObject target;

    public SkillInfo(SkillData data, GameObject skillUser, GameObject skillTarget)
    {
        skillData = data;
        user = skillUser;
        target = skillTarget;
    }
}
