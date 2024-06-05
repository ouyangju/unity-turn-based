using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractSkill : ScriptableObject
{
    public string skillsName;
    public int skillPower;

    public virtual void SkillActivate(int userAttack, CharacterStatus enemy) { }
}
