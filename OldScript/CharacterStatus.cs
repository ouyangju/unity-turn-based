using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    public string characterName;     // ��ɫ����
    public int maxHealth;      // ����ֵ
    public int currentHealth; //��ǰ����ֵ
    public int attack;      // ������
    public int defense;     // ������

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
        Debug.Log(characterName + "��ɫ�ܵ��˺�");
        currentHealth = currentHealth - skillAttack;
        currentHealth = Mathf.Max(currentHealth, 0);
    }
}
