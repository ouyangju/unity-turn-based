using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "EventData", menuName = "EventSystem/EventData")]
public class EventData : ScriptableObject
{
    public string title; // 事件标题
    public EventPage[] pages;
    public Sprite image; // 事件图片
}
