using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "SkillSystem/SkillData")]
public class SkillData : ScriptableObject
{
    //������Ϣ
    public int skillId;
    public string skillName;
    public string skillDescription;
    public float skillHitRate;//����������
    public float skillCriticalRate;//����������
    public float skillCriticalMult;//��������
    public string[] tags;//���ܵ�����
    public int skillPow;//����
    public int recoverAmount;//������
    public int skillCost;//����MP
    public int skillTimes;//ÿ��ʹ�ü����ظ����ٴι���

    public Profess skillProfess;//��������ְҵ
    public Rare skillRare;//ϡ�ж�

    public int coolDownRemain = 0;//ʣ�����ȴʱ��
    public int coolDown = 0;//ÿ��ʹ�ú����ȴ
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