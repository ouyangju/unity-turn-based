using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffData", menuName = "BuffSystem/BuffData", order = 1)]
public class BuffData : ScriptableObject
{
    //基本信息
    public int id;
    public string buffName;
    public string description;
    public string icon;
    public int priority;
    public int maxStack;
    public string[] tags;
    //持续时间的回合信息
    public bool isForever;
    public int durationTurn;
    //更新方式
    public BuffUpdateEnum buffUpdate;
    public BuffRemoveEnum buffRemove;
    //回调点
    public BaseBuffModel OnCreate;
    public BaseBuffModel OnRemove;//层数减少时触发的
    public BaseBuffModel OnDestroy;//buff彻底消失的时候触发的
    public BaseBuffModel OnAfterAction;
    public BaseBuffModel OnBeforeAction;
    //攻击的回调点
    public BaseBuffModel OnHit;
    public BaseBuffModel OnKill;
    public BaseBuffModel OnBehurt;//受击
    public BaseBuffModel OnBekill;

    //回合的回调点
    public BaseBuffModel OnTurnBegan;
    public BaseBuffModel OnTurnEnd;

    //数值减少时的回调点，记得和受伤回调点区分开来
    public BaseBuffModel OnReduceHP;
    public BaseBuffModel OnReduceMP;

    public BaseBuffModel OnHeal;
    public BaseBuffModel OnBeHeal;
}
