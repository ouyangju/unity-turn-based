using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffData", menuName = "BuffSystem/BuffData", order = 1)]
public class BuffData : ScriptableObject
{
    //������Ϣ
    public int id;
    public string buffName;
    public string description;
    public string icon;
    public int priority;
    public int maxStack;
    public string[] tags;
    //����ʱ��Ļغ���Ϣ
    public bool isForever;
    public int durationTurn;
    //���·�ʽ
    public BuffUpdateEnum buffUpdate;
    public BuffRemoveEnum buffRemove;
    //�ص���
    public BaseBuffModel OnCreate;
    public BaseBuffModel OnRemove;//��������ʱ������
    public BaseBuffModel OnDestroy;//buff������ʧ��ʱ�򴥷���
    public BaseBuffModel OnAfterAction;
    public BaseBuffModel OnBeforeAction;
    //�����Ļص���
    public BaseBuffModel OnHit;
    public BaseBuffModel OnKill;
    public BaseBuffModel OnBehurt;//�ܻ�
    public BaseBuffModel OnBekill;

    //�غϵĻص���
    public BaseBuffModel OnTurnBegan;
    public BaseBuffModel OnTurnEnd;

    //��ֵ����ʱ�Ļص��㣬�ǵú����˻ص������ֿ���
    public BaseBuffModel OnReduceHP;
    public BaseBuffModel OnReduceMP;

    public BaseBuffModel OnHeal;
    public BaseBuffModel OnBeHeal;
}
