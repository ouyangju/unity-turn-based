using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeProperty", menuName = "BuffSystem/ChangeProperty", order = 1)]
public class ChangeProperty : BaseBuffModel
{
    public int hp;
    public int mp;
    public int attack;
    public int defense;


    public override void Apply(BuffInfo buffInfo, DamageInfo damageInfo = null, RecoverInfo recoverInfo = null)
    {
        var character = buffInfo.target.GetComponent<Character>();
        if (character)
        {
            character.currentHp += hp;
            character.currentMp += mp;
            character.attack += attack;
            character.defense += defense;
        }
    }
}
