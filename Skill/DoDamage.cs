using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DoDamage", menuName = "SkillSystem/DoDamage")]
public class DoDamage : BaseSkillModel
{
    public override void ApplySkill(SkillData skill, GameObject target, GameObject user)
    {
        Character userCharacter = user.GetComponent<Character>();
        Character targetCharacter = target.GetComponent<Character>();
        int damage = skill.skillPow + userCharacter.attack - targetCharacter.defense;
        DamageInfo damageInfo = new DamageInfo(user, target, damage, skill.skillHitRate, skill.skillCriticalRate, skill.skillCriticalMult);

        for (int i = 0; i < skill.skillTimes; i++)
        {
            TurnManager.instance.AddDamageEvent(damageInfo);
        }

        //DamageManager.instance.DoDamage(damageInfo, skill.skillTimes);

    }
}
