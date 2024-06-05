using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SkillBase : AbstractSkill
{
    public override void SkillActivate(int userAttack, CharacterStatus enemy)
    {
        Debug.Log("角色开始使用技能" + skillsName);
        int skillAttack = userAttack + skillPower;
        skillAttack = Mathf.Max(skillAttack, 0);
        enemy.TakeDamage(skillAttack);
    }
}
