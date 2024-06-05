using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "EventData", menuName = "EventSystem/EventData")]
public class EventData : ScriptableObject
{
    public string title; // �¼�����
    public EventPage[] pages;
    public Sprite image; // �¼�ͼƬ
}
