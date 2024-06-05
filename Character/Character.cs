using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Character : MonoBehaviour
{
    public string characterName;
    public int maxHp;      // �������ֵ
    public int currentHp;
    public int maxMp; //���ħ��ֵ
    public int currentMp;
    public int attack;      // ������
    public int defense;     // ������

    public SkillData[] skills;
    public EquipmentData[] equipments;

    public bool isDead;
    public Dialogue dialogue;
    public Profess Profess;

    public bool isCanBeKill(DamageInfo damageInfo)
    {
        int curHp = currentHp;
        if ((curHp -= damageInfo.damage) <= 0)
        {
            return true;
        }
        else return false;
    }

    public void ReduceHP(DamageInfo damageInfo)
    {
        BuffHandler buffHandler = gameObject.GetComponent<BuffHandler>();
        foreach (var buffInfo in buffHandler.buffList)
        {
            buffInfo.buffData.OnReduceHP?.Apply(buffInfo, damageInfo, null);
        }
        currentHp -= damageInfo.damage;
        if (currentHp <= 0)
        {
            currentHp = 0;
            isDead = true;
            damageInfo.defender.tag = "DeadUnit";
        }
    }

    public void IncreaseHP(RecoverInfo recoverInfo)
    {
        currentHp += recoverInfo.recovering;
        currentHp = Mathf.Min(maxHp, currentHp);
    }

    public void CostMP(SkillData skill)
    {
        BuffHandler buffHandler = gameObject.GetComponent<BuffHandler>();
        foreach (var buffInfo in buffHandler.buffList)
        {
            buffInfo.buffData.OnReduceMP?.Apply(buffInfo, null);
        }
        currentMp -= skill.skillCost;
    }

    public void LeranSkill(SkillData skill)
    {
        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i] == null)
            {
                skills[i] = skill;
                break;
            }
        }
    }

    public void RemoveEquipment(int equipmentIndex)
    {
        equipments[equipmentIndex] = null;
        UpdateEquipmentStates();
    }

    public void AddEquipment(EquipmentData equipment)
    {
        for (int i = 0; i < equipments.Length; i++)
        {
            if (equipments[i] == null)
            {
                equipments[i] = equipment;
                UpdateEquipmentStates();
                break;
            }
        }
    }

    //����ֱ�������װ���Ĵ�����������ݣ��о��������ַ����ã�³����ǿһ�㣬Ҳ����ֱ�������������װ��
    public void UpdateEquipmentStates()
    {
        foreach (var equipment in equipments)
        {
            if (equipment != null)
            {
                maxHp += equipment.hp;
                currentHp = Mathf.Min(currentHp, maxHp);
                attack += equipment.attack;
                defense += equipment.defense;
            }
        }
    }

    //���¼��ܵ�״̬
    public void UpdateSkillState()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            bool canUseSkill = (skills[i].coolDownRemain > 0) && (skills[i].skillCost > currentMp);
            skills[i].isUseful = !canUseSkill;
        }
    }

    public void CopyDataTo(Character targetCharacter)
    {
        // ȷ��Ŀ��Characterʵ���ǿ�
        if (targetCharacter == null)
        {
            Debug.LogError("Target Character is null.");
            return;
        }

        // ���ƻ�������
        targetCharacter.characterName = this.characterName;
        targetCharacter.maxHp = this.maxHp;
        targetCharacter.currentHp = this.currentHp;
        targetCharacter.maxMp = this.maxMp;
        targetCharacter.currentMp = this.currentMp;
        targetCharacter.attack = this.attack;
        targetCharacter.defense = this.defense;
        targetCharacter.isDead = this.isDead;

        // ����������������Ϊ����Ҳ���������ͣ�ֱ�Ӹ�ֵ�ᵼ������������ͬһ������
        if (skills != null)
        {
            targetCharacter.skills = new SkillData[skills.Length];
            Array.Copy(skills, targetCharacter.skills, skills.Length);
        }
        else
        {
            targetCharacter.skills = null;
        }

        // �Ի����ƣ�����Dialogue��һ�������������Ҫ���⴦����ࣩ
        targetCharacter.dialogue = this.dialogue;

        // ְҵֱ�Ӹ�ֵ��ö����ֵ���ͣ�����ֱ�Ӹ�ֵ�����������⣩
        targetCharacter.Profess = this.Profess;

        // ע�⣺BuffHandler�����������ڲ�����Ҫ����״̬������Ҫ���������˴�δ��������ʵ��
    }
}

public enum Profess
{
    warrior,
    mamgician,
    doctor,
    assassin
}
