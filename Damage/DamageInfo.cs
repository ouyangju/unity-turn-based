using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///��Ϸ���κ�һ���˺������Ƶ��߼����������һ��damageInfo���ɴ˿�ʼ�������˺����̣�������ֱ�Ӹ�дhp
///ֵ��һ����ǣ������ơ�����ʱ��������һ���˺�������Ч���У�����һ���˺�ҲӦ����һ��damageInfo��
public class DamageInfo : TurnEvent
{
    public GameObject attacker;
    public GameObject defender;//�ܻ���

    ///<summary>
    ///����˺�������Tag������ᱻ����buff��ص��߼�����һ��������Ҫ����Ϣ
    ///�����ǲ߻�������Ϸ���������ģ�������Ϸ�п��ܴ���"frozen" "fire"֮����˺����ͣ��������"directDamage" "period" "reflect"֮��������˺�
    ///������Щ�˺����ͣ��߼�������ܻ�������ͬ�����͵ı���"reflect"�����Է��˵ģ��Ǳ���һ��buff�����þ����ܵ��˺���ʱ�򷴵��˺������˫���������buff
    ///�������buffû���ж�damageInfo.tags������reflect���������ɡ���·����������һ����һ�������ˡ�
    ///</summary>
    public DamageInfoTag tags;
    public DamageInfoElement elements;

    public int damage;
    public float criticalRate;
    public float criticalMult;
    public float hitRate;

    public DamageInfo(DamageInfo damageInfo)
    {
        this.attacker = damageInfo.attacker;
        this.defender = damageInfo.defender;

        this.tags = damageInfo.tags;
        this.elements = damageInfo.elements;

        this.damage = damageInfo.damage;
        this.criticalRate = damageInfo.criticalRate;
        this.criticalMult = damageInfo.criticalMult;
        this.hitRate = damageInfo.hitRate;

    }

    public DamageInfo(GameObject attacker, GameObject defender, int damage, float hitRate = 1, float criticalRate = 0, float criticalMult = 2, DamageInfoTag tags = DamageInfoTag.Direct)
    {
        this.attacker = attacker;
        this.defender = defender;
        this.damage = damage;
        this.criticalRate = criticalRate;
        this.criticalMult = criticalMult;
        this.hitRate = hitRate;
        this.tags = tags;
    }
}

public enum DamageInfoTag
{
    Direct = 0,
    Buff = 1
}

public enum DamageInfoElement
{
    Null = 0,
    Fire = 1,
}