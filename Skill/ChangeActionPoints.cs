using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeActionPoints", menuName = "SkillSystem/ChangeActionPoints")]
public class ChangeActionPoints : BaseSkillModel
{
    public int AP;

    public override void ApplySkill(SkillData skill, GameObject target, GameObject user)
    {
        TurnManager.instance.PlayerActionPoints += AP;
    }
}
