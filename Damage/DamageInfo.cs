using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///游戏中任何一次伤害、治疗等逻辑，都会产生一条damageInfo，由此开始正常的伤害流程，而不是直接改写hp
///值得一提的是，在类似“攻击时产生额外一次伤害”这种效果中，额外一次伤害也应该是一个damageInfo。
public class DamageInfo : TurnEvent
{
    public GameObject attacker;
    public GameObject defender;//受击者

    ///<summary>
    ///这次伤害的类型Tag，这个会被用于buff相关的逻辑，是一个极其重要的信息
    ///这里是策划根据游戏设计来定义的，比如游戏中可能存在"frozen" "fire"之类的伤害类型，还会存在"directDamage" "period" "reflect"之类的类型伤害
    ///根据这些伤害类型，逻辑处理可能会有所不同，典型的比如"reflect"，来自反伤的，那本身一个buff的作用就是受到伤害的时候反弹伤害，如果双方都有这个buff
    ///并且这个buff没有判断damageInfo.tags里面有reflect，则可能造成“短路”，最终有一下有一方就秒了。
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