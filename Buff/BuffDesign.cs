using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffUpdateEnum
{
    Add,
    Replace,
    Keep
}

public enum BuffRemoveEnum
{
    Clear,
    Reduce
}

public class BuffInfo : TurnEvent
{
    public BuffData buffData;
    public GameObject creator;
    public GameObject target;
    public int remainTurn;//����ʱ�䣬�Ǹ���ʱ����Ҳ������Ϊ��Ŀǰ��ʣ����ʱ��
    public int currentStack = 1;

    public BuffInfo(BuffData Data, GameObject buffTarget, int curStack = 1, GameObject buffCreator = null)
    {
        buffData = Data;
        target = buffTarget;
        creator = buffCreator;
        currentStack = curStack;
    }
}