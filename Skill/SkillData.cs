using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "SkillSystem/SkillData")]
public class SkillData : ScriptableObject
{
    //基本信息
    public int skillId;
    public string skillName;
    public string skillDescription;
    public float skillHitRate;//基础暴击率
    public float skillCriticalRate;//基础命中率
    public float skillCriticalMult;//暴击倍率
    public string[] tags;//技能的种类
    public int skillPow;//威力
    public int recoverAmount;//治疗量
    public int skillCost;//消耗MP
    public int skillTimes;//每次使用技能重复多少次攻击

    public Profess skillProfess;//技能所属职业
    public Rare skillRare;//稀有度

    public int coolDownRemain = 0;//剩余的冷却时间
    public int coolDown = 0;//每次使用后的冷却
    public bool isUseful = true;

    public int level;
    public BuffData buffAttached;

    public BaseSkillModel skillActive;
}

public enum Rare
{
    Normal,
    Rare,
    Epic,
    Legend
}