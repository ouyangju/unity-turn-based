using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "EventButton", menuName = "EventSystem/EventButton")]
public class EventButton : ScriptableObject
{
    public string buttonText; // 按钮上的文字
    public EventData nextEvent; // 点击后跳转的下一个事件（如果是推进事件）
    public bool isNextPage;
    public bool isEndEvent;
    public int targetPage;
    public BaseEventModel buttonActive;
}
