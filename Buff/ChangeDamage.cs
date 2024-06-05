using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeDamage", menuName = "BuffSystem/ChangeDamage", order = 1)]
public class ChangeDamage : BaseBuffModel
{
    public float times;

    public override void Apply(BuffInfo buffInfo, DamageInfo damageInfo = null, RecoverInfo recoverInfo = null)
    {
        damageInfo.damage = (int)(damageInfo.damage * times);
    }
}
