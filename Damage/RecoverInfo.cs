using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverInfo : MonoBehaviour
{
    public GameObject user;
    public GameObject target;//ÊÜ»÷Õß

    public DamageInfoTag tags;
    public DamageInfoElement elements;

    public int recovering;

    public RecoverInfo(GameObject attacker, GameObject target, int healingAmount, DamageInfoTag tags = DamageInfoTag.Direct)
    {
        this.user = attacker;
        this.target = target;
        this.recovering = healingAmount;
        this.tags = tags;
    }
}
