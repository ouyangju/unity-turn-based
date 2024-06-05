using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBuffModel : ScriptableObject
{
    public abstract void Apply(BuffInfo buffInfo, DamageInfo damageInfo = null, RecoverInfo recoverInfo = null);
}
