using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//游戏中具体使用时的技能数据
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
