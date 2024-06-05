using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SkillBase : AbstractSkill
{
    public override void SkillActivate(int userAttack, CharacterStatus enemy)
    {
        Debug.Log("��ɫ��ʼʹ�ü���" + skillsName);
        int skillAttack = userAttack + skillPower;
        skillAttack = Mathf.Max(skillAttack, 0);
        enemy.TakeDamage(skillAttack);
    }
}
