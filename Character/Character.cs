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
    public int maxHp;      // 最大生命值
    public int currentHp;
    public int maxMp; //最大魔力值
    public int currentMp;
    public int attack;      // 攻击力
    public int defense;     // 防御力

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

    //比起直接在添加装备的代码里更新数据，感觉还是这种方法好，鲁棒性强一点，也可以直接在引擎内添加装备
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

    //更新技能的状态
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
        // 确保目标Character实例非空
        if (targetCharacter == null)
        {
            Debug.LogError("Target Character is null.");
            return;
        }

        // 复制基本属性
        targetCharacter.characterName = this.characterName;
        targetCharacter.maxHp = this.maxHp;
        targetCharacter.currentHp = this.currentHp;
        targetCharacter.maxMp = this.maxMp;
        targetCharacter.currentMp = this.currentMp;
        targetCharacter.attack = this.attack;
        targetCharacter.defense = this.defense;
        targetCharacter.isDead = this.isDead;

        // 技能数组的深拷贝，因为数组也是引用类型，直接赋值会导致两个对象共享同一份数据
        if (skills != null)
        {
            targetCharacter.skills = new SkillData[skills.Length];
            Array.Copy(skills, targetCharacter.skills, skills.Length);
        }
        else
        {
            targetCharacter.skills = null;
        }

        // 对话复制（假设Dialogue是一个可以深拷贝或不需要特殊处理的类）
        targetCharacter.dialogue = this.dialogue;

        // 职业直接赋值（枚举是值类型，所以直接赋值不会引起问题）
        targetCharacter.Profess = this.Profess;

        // 注意：BuffHandler等组件如果存在并且需要复制状态，则需要单独处理，此处未给出具体实现
    }
}

public enum Profess
{
    warrior,
    mamgician,
    doctor,
    assassin
}
