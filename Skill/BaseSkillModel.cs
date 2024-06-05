using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkillModel : ScriptableObject
{
    public abstract void ApplySkill(SkillData skill, GameObject target, GameObject user);
}
