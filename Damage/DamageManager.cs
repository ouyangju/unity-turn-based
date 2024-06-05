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
                //�ǵ��Ժ�Ӹ����ܵĻص���
                BloodFloat.instance.ShowDamage("Miss!", damageNow.defender);
                continue;
            }

            if (attackerBuffHandler)//�����ߵ�buff
            {
                foreach (var buffInfo in attackerBuffHandler.buffList)
                {
                    buffInfo.buffData.OnHit?.Apply(buffInfo, damageNow, null);
                }
            }
            if (defenderBuffHandler)//�����ߵ�buff
            {
                foreach (var buffInfo in defenderBuffHandler.buffList)
                {
                    buffInfo.buffData.OnBehurt?.Apply(buffInfo, damageNow, null);
                }

                //˫����buff�����˿�ʼ��Ⱪ���ˣ��ǵüӸ������ͱ����Ļص���
                float criticalChance = Random.Range(0f, 1f);
                if (criticalChance < damageNow.criticalRate)
                {
                    Debug.Log("������");
                    damageNow.damage = (int)(damageNow.damage * damageNow.criticalMult);
                }

                if (defenderCharacter)//����Ƿ������
                {
                    if (defenderCharacter.isCanBeKill(damageNow))//�����Ժ󴥷���
                    {
                        foreach (var buffInfo in defenderBuffHandler.buffList)
                        {
                            buffInfo.buffData.OnBekill?.Apply(buffInfo, damageNow, null);
                        }

                        if (defenderCharacter.isCanBeKill(damageNow))//�ټ��һ�Σ���Ϊ����������
                        {
                            if (attackerBuffHandler)//ȷ�������Ժ󴥷���ɱ��Ч
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
            //�˺����㣬��δ����
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
