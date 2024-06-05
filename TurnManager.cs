using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;

    //�������˵�Ԥ����
    public GameObject[] playerPrefabs;
    public GameObject[] enemyPrefabs;

    public Transform[] playerStations;
    public Transform[] enemyStations;

    private GameObject[] playerUnit;
    private GameObject[] enemyUnit;

    public Text battleDialogue;
    public Dialogue dialogue;//�Ի�����;
    public GameObject dialogBox;
    public BattleUI battleUI;

    public BattleStates states;
    private bool isGameOver = false;
    public int PlayerActionPoints;
    public int EnemyActionPoints;

    //��Ϸ����ʱ���������
    private GameObject currentActUnit;
    private GameObject currentTargetUnit;
    private int currentActUnitIndex;
    private int currentTargetUnitIndex;
    private int currentSkillIndex;

    //ѡ����󻷽���ر���
    private bool IsPlayerChooseTarget = false;
    private bool IsPlayerChooseTalk = false;
    private Ray targetChooseRay;            //���ѡ�񹥻����������
    private RaycastHit targetHit;           //����Ŀ��

    private List<TurnEvent> TurnEvents = new List<TurnEvent>();

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        for (int i = 0; i < LevelDataManager.Instance.playerCharacter.Length; i++)
        {
            playerPrefabs[i] = LevelDataManager.Instance.playerCharacter[i];
        }

        battleDialogue.text = "Battle Start!";
        StartCoroutine(DealWithTurnEvent());
        StartCoroutine(SetUpBattle());
    }

    IEnumerator SetUpBattle()
    {
        //��ʼ��˫��������
        playerUnit = new GameObject[playerStations.Length];
        enemyUnit = new GameObject[enemyStations.Length];


        //���ɵ�λ
        SetPlayerUnits();
        SetEnemyUnits();

        currentActUnitIndex = 0;
        currentActUnit = playerUnit[currentActUnitIndex];

        yield return new WaitForSeconds(1f);
        battleDialogue.text = "Choose your skill!";
        PlayerActionPoints = 4;

        battleUI.TurnPanel.GetComponentInChildren<Text>().text = "Player Turn";
        battleUI.FadeInAndOut();
        battleUI.SetBattleUI(currentActUnit);

        PlayerTurnCircle();
    }

    public void SetPlayerUnits()
    {
        for (int i = 0; i < playerPrefabs.Length; i++)
        {
            //ʵ�������
            GameObject playerGameObject = Instantiate(playerPrefabs[i], playerStations[i]);
            playerUnit[i] = playerGameObject;
        }
    }

    public void SetEnemyUnits()
    {
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            GameObject enemyGameObject = Instantiate(enemyPrefabs[i], enemyStations[i]);
            enemyUnit[i] = enemyGameObject;
        }
    }

    //�غϿ�ʼʱ��buff����,�Ժ�ǵüӸ��������
    public void BuffOnTurnBegin(GameObject[] Units)
    {
        for (int i = 0; i < Units.Length; i++)
        {
            BuffHandler buffHandler = Units[i].GetComponent<BuffHandler>();
            foreach (var buffInfo in buffHandler.buffList)
            {
                buffInfo.buffData.OnTurnBegan?.Apply(buffInfo, null);
            }
        }
    }

    //��һغ�ѭ����Ҳ�ǻ�û���������
    public void PlayerTurnCircle()
    {
        CheckVictory();
        if (!isGameOver)
        {
            //�ж��������л��غ�
            if (PlayerActionPoints <= 0)
            {
                //����غϽ�����buff
                BuffOnTurnEnd(playerUnit);
                BuffDuationReduce(playerUnit);

                //���³�ʼ������
                EnemyActionPoints = 4;
                currentActUnitIndex = 0;
                currentActUnit = enemyUnit[currentActUnitIndex];
                states = BattleStates.ENEMYTURN;
                battleDialogue.text = "enemy turn now��";

                //����UI
                battleUI.TurnPanel.GetComponentInChildren<Text>().text = "Enemy Turn";
                battleUI.FadeInAndOut();

                //�з��غϿ�ʼ
                BuffOnTurnBegin(playerUnit);
                StartCoroutine(EnemyTurnCircle());
                return;
            }

            if (currentActUnit.GetComponent<Character>().isDead == false)
            {
                states = BattleStates.PLAYERTURN;
                battleUI.SetBattleUI(currentActUnit);
                Debug.Log("�ҷ���ʣ" + PlayerActionPoints + "���ж���");
                currentActUnit = playerUnit[currentActUnitIndex];
                Debug.Log("�����ǵ�" + currentActUnitIndex + "λ��ҽ�ɫ���ж�");
            }
            else
            {
                currentActUnitIndex = (currentActUnitIndex + 1) % playerUnit.Length;
                currentActUnit = playerUnit[currentActUnitIndex];
                PlayerTurnCircle();
            }
        }
    }

    IEnumerator EnemyTurnCircle()
    {
        CheckVictory();
        if (!isGameOver)
        {
            //�ж��������л��غ�
            if (EnemyActionPoints == 0)
            {
                BuffDuationReduce(enemyUnit);

                //���³�ʼ������
                PlayerActionPoints = 4;
                currentActUnitIndex = 0;
                currentActUnit = playerUnit[currentActUnitIndex];
                states = BattleStates.PLAYERTURN;
                battleDialogue.text = "player turn now��";

                //����UI
                battleUI.TurnPanel.GetComponentInChildren<Text>().text = "Player Turn";
                battleUI.FadeInAndOut();

                //�ҷ��غϿ�ʼ
                BuffOnTurnBegin(playerUnit);
                PlayerTurnCircle();
                yield break;
            }

            if (currentActUnit.GetComponent<Character>().isDead == false)
            {
                states = BattleStates.ENEMYTURN;
                battleUI.SetBattleUI(currentActUnit);


                Debug.Log("���˻�ʣ" + EnemyActionPoints + "���ж���");
                currentActUnit = enemyUnit[currentActUnitIndex];

                // �������ѡ����Ŀ��ͼ���
                bool foundValidTarget = false;
                do
                {
                    currentTargetUnitIndex = Random.Range(0, playerUnit.Length);
                    currentTargetUnit = playerUnit[currentTargetUnitIndex];
                    Character targetCharacter = currentTargetUnit.GetComponent<Character>();
                    foundValidTarget = targetCharacter != null && !targetCharacter.isDead;
                } while (!foundValidTarget);

                currentSkillIndex = Random.Range(0, currentActUnit.GetComponent<Character>().skills.Length);
                yield return new WaitForSeconds(2f);
                EnemyUseSkill();
            }
            else //�����������ѭ���ҵ���һ�����ŵ�
            {
                Debug.Log("�����ǵ�" + currentActUnitIndex + "λ����" + currentActUnit.GetComponent<Character>().name + "��������");
                currentActUnitIndex = (currentActUnitIndex + 1) % enemyUnit.Length;
                currentActUnit = enemyUnit[currentActUnitIndex];
                StartCoroutine(EnemyTurnCircle());
            }
        }
    }

    public void OnSkillButton(int index)
    {
        if (states != BattleStates.PLAYERTURN) return;
        currentSkillIndex = index;
        Debug.Log("�ѵ�����ܰ�ť" + currentSkillIndex);
        IsPlayerChooseTarget = true;
    }

    public void OnDialogButton()
    {
        if (states != BattleStates.PLAYERTURN) return;
        Debug.Log("�ѵ���Ի���ť");
        IsPlayerChooseTalk = true;
    }

    void Update()
    {
        if (IsPlayerChooseTarget)
        {
            targetChooseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(targetChooseRay, out targetHit))
            {
                if (Input.GetMouseButtonDown(0) && targetHit.collider.gameObject.CompareTag("EnemyUnit") && !targetHit.collider.gameObject.GetComponent<Character>().isDead)
                {
                    currentTargetUnit = targetHit.collider.gameObject;
                    Debug.Log("��ѡ��" + currentTargetUnit.GetComponent<Character>().name);
                    UseSkill();
                }
            }
        }
        if (IsPlayerChooseTalk)
        {
            targetChooseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(targetChooseRay, out targetHit))
            {
                if (Input.GetMouseButtonDown(0) && targetHit.collider.gameObject.CompareTag("EnemyUnit"))
                {
                    currentTargetUnit = targetHit.collider.gameObject;
                    dialogue = currentActUnit.GetComponent<Character>().dialogue;
                    dialogBox.SetActive(true);
                    Debug.Log("��ѡ��" + currentTargetUnit.GetComponent<Character>().name);
                }
            }
        }
    }

    public void BuffOnBeforeAction()
    {
        BuffHandler buffHandler = currentActUnit.GetComponent<BuffHandler>();
        foreach (var buffInfo in buffHandler.buffList)
        {
            buffInfo.buffData.OnBeforeAction?.Apply(buffInfo, null);
        }
    }

    public void UseSkill()
    {
        Character attackerCharacter = currentActUnit.GetComponent<Character>();
        battleDialogue.text = attackerCharacter.characterName + "��" + currentTargetUnit.GetComponent<Character>().characterName + "ʹ����" + attackerCharacter.skills[currentSkillIndex].skillName;

        BuffOnBeforeAction();
        SkillInfo skillInfo = new SkillInfo(attackerCharacter.skills[currentSkillIndex], currentActUnit, currentTargetUnit);
        TurnEvents.Add(skillInfo);
        //���ﱾ���и�setui�ĺ���������ǵõ���һ��

        //����ѭ��
        IsPlayerChooseTarget = false;
        currentActUnitIndex = (currentActUnitIndex + 1) % playerUnit.Length;
        currentActUnit = playerUnit[currentActUnitIndex];
        PlayerActionPoints--;
        PlayerTurnCircle();
    }

    public void EnemyUseSkill()
    {
        Character attackerCharacter = currentActUnit.GetComponent<Character>();
        battleDialogue.text = attackerCharacter.characterName + "��" + currentTargetUnit.GetComponent<Character>().characterName + "ʹ����" + attackerCharacter.skills[currentSkillIndex].skillName;

        BuffOnBeforeAction();
        SkillInfo skillInfo = new SkillInfo(attackerCharacter.skills[currentSkillIndex], currentActUnit, currentTargetUnit);
        TurnEvents.Add(skillInfo);
        //���ﱾ���и�setui�ĺ���������ǵõ���һ��

        //����ѭ��
        currentActUnitIndex = (currentActUnitIndex + 1) % enemyUnit.Length;
        currentActUnit = enemyUnit[currentActUnitIndex];
        EnemyActionPoints--;
        StartCoroutine(EnemyTurnCircle());
    }

    public void BuffOnAfterUseSkill(GameObject user)
    {
        BuffHandler buffHandler = user.GetComponent<BuffHandler>();
        Debug.Log(user.GetComponent<Character>().characterName+"������һ���ж���buff��");
        foreach (var buffInfo in buffHandler.buffList)
        {
            buffInfo.buffData.OnAfterAction?.Apply(buffInfo, null);
        }
    }

    public void BuffOnAfterAction()
    {
        BuffHandler buffHandler = currentActUnit.GetComponent<BuffHandler>();
        foreach (var buffInfo in buffHandler.buffList)
        {
            buffInfo.buffData.OnAfterAction?.Apply(buffInfo, null);
        }
    }

    public void BuffOnTurnEnd(GameObject[] Units)
    {
        for (int i = 0; i < Units.Length; i++)
        {
            BuffHandler buffHandler = Units[i].GetComponent<BuffHandler>();
            foreach (var buffInfo in buffHandler.buffList)
            {
                buffInfo.buffData.OnTurnEnd?.Apply(buffInfo, null);
            }
        }
    }

    public void BuffDuationReduce(GameObject[] Units)
    {
        for (int i = 0; i < Units.Length; i++)
        {
            List<BuffInfo> deleteBuffList = new List<BuffInfo>();
            BuffHandler buffHandler = Units[i].GetComponent<BuffHandler>();
            foreach (var buffInfo in buffHandler.buffList)
            {
                if (!buffInfo.buffData.isForever)
                {
                    buffInfo.remainTurn--;
                    if (buffInfo.remainTurn == 0)
                    {
                        deleteBuffList.Add(buffInfo);
                    }
                }
            }
            foreach (var buffInfo in deleteBuffList)
            {
                buffHandler.RemoveBuff(buffInfo);
            }
        }
    }

    public void CheckVictory()
    {
        // �������Ƿ�ȫ������
        bool allPlayersDead = true;
        foreach (GameObject player in playerUnit)
        {
            Character playerCharacter = player.GetComponent<Character>();
            if (!playerCharacter.isDead)
            {
                allPlayersDead = false;
                break;
            }
        }

        // �������Ƿ�ȫ������
        bool allEnemiesDead = true;
        foreach (GameObject enemy in enemyUnit)
        {
            Character enemyCharacter = enemy.GetComponent<Character>();
            if (!enemyCharacter.isDead)
            {
                allEnemiesDead = false;
                break;
            }
        }

        if (allEnemiesDead)
        {
            Debug.Log("���ʤ����");
            isGameOver = true;
            LevelDataManager.Instance.LoadUI();
            // ������������ʤ������߼������絯��ʤ�����桢����ʤ��������
        }
        else if (allPlayersDead)
        {
            Debug.Log("���ʧ�ܣ�");
            isGameOver = true;
            // ���ʧ�ܺ���߼�������Ϸ�������桢����ѡ���
        }
    }

    public void AddDamageEvent(DamageInfo damageInfo)
    {
        TurnEvents.Add(damageInfo);
    }

    public void AddBuffEvent(BuffInfo buffInfo)
    {
        TurnEvents.Add(buffInfo);
    }

    private IEnumerator DealWithTurnEvent()
    {
        while (true)
        {
            yield return new WaitUntil(() => TurnEvents.Count > 0);
            int i = 0;
            while (TurnEvents.Count > i)
            {
                TurnEvent currentEvent = TurnEvents[i];
                if (currentEvent is SkillInfo skillInfo)
                {
                    skillInfo.skillData.skillActive.ApplySkill(skillInfo.skillData, skillInfo.target, skillInfo.user);
                    TurnEvents.RemoveAt(0);
                    yield return new WaitForSeconds(0.2f);

                    BuffOnAfterUseSkill(skillInfo.user);
                }
                else if (currentEvent is DamageInfo damageInfo)
                {
                    DoDamage(damageInfo);
                    TurnEvents.RemoveAt(0);
                    yield return new WaitForSeconds(0.2f);
                }
                else if(currentEvent is BuffInfo buffInfo)
                {
                    BuffHandler targetBuffHandler = buffInfo.target.GetComponent<BuffHandler>();
                    targetBuffHandler.AddBuff(buffInfo);
                    TurnEvents.RemoveAt(0);
                }
            }
        }
    }

    public void DoDamage(DamageInfo damageInfo)
    {
        if (damageInfo.tags == DamageInfoTag.Direct)
        {
            damageInfo.attacker.GetComponent<CharacterAnimation>()?.CastAttack();
        }

        BuffHandler attackerBuffHandler = damageInfo.attacker?.GetComponent<BuffHandler>();
        BuffHandler defenderBuffHandler = damageInfo.defender?.GetComponent<BuffHandler>();
        var defenderCharacter = damageInfo.defender.GetComponent<Character>();

        damageInfo.damage = Mathf.Max(damageInfo.damage, 0);
        DamageInfo damageNow = new DamageInfo(damageInfo);

        float hitChance = Random.Range(0f, 1f);
        if (hitChance > damageNow.hitRate)
        {
            //�ǵ��Ժ�Ӹ����ܵĻص���
            BloodFloat.instance.ShowDamage("Miss!", damageNow.defender);
            return;

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
    }

}
