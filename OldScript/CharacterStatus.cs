using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    public string characterName;     // 角色名称
    public int maxHealth;      // 生命值
    public int currentHealth; //当前生命值
    public int attack;      // 攻击力
    public int defense;     // 防御力

    public AbstractSkill[] characterSkill = new AbstractSkill[8];

    public void AddSkill(AbstractSkill skillToAdd)
    {
        for (int i = 0; i < characterSkill.Length; i++)
        {
            if (characterSkill[i] == null)
            {
                characterSkill[i] = skillToAdd;
                break;
            }
        }
    }

    public void TakeDamage(int skillAttack)
    {
        Debug.Log(characterName + "角色受到伤害");
        currentHealth = currentHealth - skillAttack;
        currentHealth = Mathf.Max(currentHealth, 0);
    }
}
