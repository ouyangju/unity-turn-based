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
        if (findBuffInfo != null) //如果buff存在
        {
            if (findBuffInfo.currentStack < findBuffInfo.buffData.maxStack)//如果没叠满
            {
                findBuffInfo.currentStack++;
                findBuffInfo.buffData.OnCreate?.Apply(findBuffInfo, null);//创建时触发的Buff
                BloodFloat.instance.ShowBuff(buffInfo);
            }
            switch ((findBuffInfo.buffData.buffUpdate))//不论叠没叠满都触发持续时间的变更
            {
                case BuffUpdateEnum.Add:
                    findBuffInfo.remainTurn += findBuffInfo.buffData.durationTurn;
                    break;

                case BuffUpdateEnum.Replace:
                    findBuffInfo.remainTurn = findBuffInfo.buffData.durationTurn;
                    break;
            }
        }
        else//如果buff不存在
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
            return; // 如果链表为空或只有一个元素，则无需排序
        }

        LinkedListNode<BuffInfo> current = list.First.Next; // 从第二个元素开始遍历
        while (current != null)
        {
            LinkedListNode<BuffInfo> prev = current.Previous;
            LinkedListNode<BuffInfo> next = current.Next;

            // 将 current 插入到已排序的部分
            while (prev != null && prev.Value.buffData.priority > current.Value.buffData.priority)
            {
                prev = prev.Previous;
            }

            if (prev == null) // current 的 priority 是最小的，应该放在链表的头部
            {
                list.Remove(current);
                list.AddFirst(current);
            }
            else if (prev == current.Previous) // current 的 priority 在已排序的部分中间
            {
                if (next != null)
                {
                    list.Remove(current);
                    list.AddAfter(prev, current);
                }
            }
            else // current 的 priority 是最大的，应该放在链表的尾部
            {
                list.Remove(current);
                list.AddLast(current);
            }

            current = next; // 移动到下一个节点继续遍历
        }
    }
}
