using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEvent
{
    public EventType Type;

}

public enum EventType
{
    Skill,
    Damage,
    Recover
}
