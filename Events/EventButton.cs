using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "EventButton", menuName = "EventSystem/EventButton")]
public class EventButton : ScriptableObject
{
    public string buttonText; // ��ť�ϵ�����
    public EventData nextEvent; // �������ת����һ���¼���������ƽ��¼���
    public bool isNextPage;
    public bool isEndEvent;
    public int targetPage;
    public BaseEventModel buttonActive;
}
