using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffHandler : MonoBehaviour
{
    public LinkedList<BuffInfo> buffList = new LinkedList<BuffInfo>();


    public void AddBuff(BuffInfo buffInfo)
    {
        BuffInfo findBuffInfo = FindBuff(buffInfo.buffData.id);
        if (findBuffInfo != null) //���buff����
        {
            if (findBuffInfo.currentStack < findBuffInfo.buffData.maxStack)//���û����
            {
                findBuffInfo.currentStack++;
                findBuffInfo.buffData.OnCreate?.Apply(findBuffInfo, null);//����ʱ������Buff
                BloodFloat.instance.ShowBuff(buffInfo);
            }
            switch ((findBuffInfo.buffData.buffUpdate))//���۵�û��������������ʱ��ı��
            {
                case BuffUpdateEnum.Add:
                    findBuffInfo.remainTurn += findBuffInfo.buffData.durationTurn;
                    break;

                case BuffUpdateEnum.Replace:
                    findBuffInfo.remainTurn = findBuffInfo.buffData.durationTurn;
                    break;
            }
        }
        else//���buff������
        {
            buffInfo.remainTurn = buffInfo.buffData.durationTurn;
            buffInfo.buffData.OnCreate?.Apply(buffInfo, null);
            buffList.AddLast(buffInfo);
            BloodFloat.instance.ShowBuff(buffInfo);
            SortBuffListByPriority(buffList);
        }
    }

    public void RemoveBuff(BuffInfo buffInfo)
    {
        switch (buffInfo.buffData.buffRemove)
        {
            case BuffRemoveEnum.Clear:
                buffInfo.buffData.OnRemove?.Apply(buffInfo, null);
                buffList.Remove(buffInfo);
                break;

            case BuffRemoveEnum.Reduce:
                buffInfo.currentStack--;
                buffInfo.buffData.OnRemove?.Apply(buffInfo, null);
                if (buffInfo.currentStack <= 0)
                {
                    buffList.Remove(buffInfo);
                }
                else
                {
                    buffInfo.remainTurn = buffInfo.buffData.durationTurn;
                }
                break;
        }
    }

    private BuffInfo FindBuff(int buffDataID)
    {
        foreach (var buffInfo in buffList)
        {
            if (buffInfo.buffData.id == buffDataID)
            {
                return buffInfo;
            }
        }

        return default;
    }

    public void SortBuffListByPriority(LinkedList<BuffInfo> list)
    {
        if (list == null || list.First == null)
        {
            return; // �������Ϊ�ջ�ֻ��һ��Ԫ�أ�����������
        }

        LinkedListNode<BuffInfo> current = list.First.Next; // �ӵڶ���Ԫ�ؿ�ʼ����
        while (current != null)
        {
            LinkedListNode<BuffInfo> prev = current.Previous;
            LinkedListNode<BuffInfo> next = current.Next;

            // �� current ���뵽������Ĳ���
            while (prev != null && prev.Value.buffData.priority > current.Value.buffData.priority)
            {
                prev = prev.Previous;
            }

            if (prev == null) // current �� priority ����С�ģ�Ӧ�÷��������ͷ��
            {
                list.Remove(current);
                list.AddFirst(current);
            }
            else if (prev == current.Previous) // current �� priority ��������Ĳ����м�
            {
                if (next != null)
                {
                    list.Remove(current);
                    list.AddAfter(prev, current);
                }
            }
            else // current �� priority �����ģ�Ӧ�÷��������β��
            {
                list.Remove(current);
                list.AddLast(current);
            }

            current = next; // �ƶ�����һ���ڵ��������
        }
    }
}
