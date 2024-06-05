using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public static DamageManager instance;

    public void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DoDamage(DamageInfo damageInfo, int times = 1)
    {
        StartCoroutine(StartDoDamage(damageInfo, times));
        TurnManager.instance.CheckVictory();
    }

    public IEnumerator StartDoDamage(DamageInfo damageInfo, int times = 1)
    {
        if (damageInfo.tags == DamageInfoTag.Direct)
        {
            damageInfo.attacker.GetComponent<CharacterAnimation>()?.CastAttack();
        }

        BuffHandler attackerBuffHandler = damageInfo.attacker?.GetComponent<BuffHandler>();
        BuffHandler defenderBuffHandler = damageInfo.defender?.GetComponent<BuffHandler>();
        var defenderCharacter = damageInfo.defender.GetComponent<Character>();
        damageInfo.damage = Mathf.Max(damageInfo.damage, 0);

        for (int i = 0; i < times; i++)
        {
            DamageInfo damageNow = new DamageInfo(damageInfo);
            float hitChance = Random.Range(0f, 1f);
            if (hitChance > damageNow.hitRate)
            {
                //记得以后加个闪避的回调点
                BloodFloat.instance.ShowDamage("Miss!", damageNow.defender);
                continue;
            }

            if (attackerBuffHandler)//攻击者的buff
            {
                foreach (var buffInfo in attackerBuffHandler.buffList)
                {
                    buffInfo.buffData.OnHit?.Apply(buffInfo, damageNow, null);
                }
            }
            if (defenderBuffHandler)//防御者的buff
            {
                foreach (var buffInfo in defenderBuffHandler.buffList)
                {
                    buffInfo.buffData.OnBehurt?.Apply(buffInfo, damageNow, null);
                }

                //双方的buff过完了开始检测暴击了，记得加个暴击和被爆的回调点
                float criticalChance = Random.Range(0f, 1f);
                if (criticalChance < damageNow.criticalRate)
                {
                    Debug.Log("暴击！");
                    damageNow.damage = (int)(damageNow.damage * damageNow.criticalMult);
                }

                if (defenderCharacter)//检测是否会死亡
                {
                    if (defenderCharacter.isCanBeKill(damageNow))//似了以后触发的
                    {
                        foreach (var buffInfo in defenderBuffHandler.buffList)
                        {
                            buffInfo.buffData.OnBekill?.Apply(buffInfo, damageNow, null);
                        }

                        if (defenderCharacter.isCanBeKill(damageNow))//再检测一次，因为可能有免死
                        {
                            if (attackerBuffHandler)//确定似了以后触发击杀特效
                            {
                                foreach (var buffInfo in attackerBuffHandler.buffList)
                                {
                                    buffInfo.buffData.OnKill?.Apply(buffInfo, damageNow, null);
                                }
                            }
                        }
                    }
                }
            }
            //伤害计算，还未完善
            int hpBeforeDamage = defenderCharacter.currentHp;
            defenderCharacter.ReduceHP(damageNow);
            damageNow.defender.GetComponent<CharacterAnimation>()?.OnHit();
            damageNow.defender.GetComponent<HealthBar>()?.UpdateHealthBar();
            string damageString = (hpBeforeDamage - defenderCharacter.currentHp).ToString();
            BloodFloat.instance.ShowDamage(damageString, damageNow.defender);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
