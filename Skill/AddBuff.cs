using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AddBuff", menuName = "SkillSystem/AddBuff")]
public class AddBuff : BaseSkillModel
{
    public override void ApplySkill(SkillData skill, GameObject target, GameObject user)
    {
        BuffInfo buffInfo = new BuffInfo(skill.buffAttached, target);
        TurnManager.instance.AddBuffEvent(buffInfo);

    }
}
