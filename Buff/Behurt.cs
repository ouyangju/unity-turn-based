using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Behurt", menuName = "BuffSystem/Behurt", order = 1)]
public class Behurt : BaseBuffModel
{
    public int damageData;
    public override void Apply(BuffInfo buffInfo, DamageInfo damageInfo = null, RecoverInfo recoverInfo = null)
    {
        var character = buffInfo.target.GetComponent<Character>();
        if (character)
        {
            DamageInfo damage = new DamageInfo(buffInfo.creator, buffInfo.target, damageData, 1, 0, 1, DamageInfoTag.Buff);
            TurnManager.instance.AddDamageEvent(damage);
            Debug.Log("buff‘Ï≥……À∫¶£°");
        }
    }
}