using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventPage", menuName = "EventSystem/EventPage")]
public class EventPage : ScriptableObject
{
    public string descriptionText;
    public EventButton[] eventButtons;
}
