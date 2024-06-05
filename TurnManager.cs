using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;

    //玩家与敌人的预制体
    public GameObject[] playerPrefabs;
    public GameObject[] enemyPrefabs;

    public Transform[] playerStations;
    public Transform[] enemyStations;

    private GameObject[] playerUnit;
    private GameObject[] enemyUnit;

    public Text battleDialogue;
    public Dialogue dialogue;//对话内容;
    public GameObject dialogBox;
    public BattleUI battleUI;

    public BattleStates states;
    private bool isGameOver = false;
    public int PlayerActionPoints;
    public int EnemyActionPoints;

    //游戏运行时的物体变量
    private GameObject currentActUnit;
    private GameObject currentTargetUnit;
    private int currentActUnitIndex;
    private int currentTargetUnitIndex;
    private int currentSkillIndex;

    //选择对象环节相关变量
    private bool IsPlayerChooseTarget = false;
    private bool IsPlayerChooseTalk = false;
    private Ray targetChooseRay;            //玩家选择攻击对象的射线
    private RaycastHit targetHit;           //射线目标

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
        //初始化双方的人数
        playerUnit = new GameObject[playerStations.Length];
        enemyUnit = new GameObject[enemyStations.Length];


        //生成单位
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
            //实例化玩家
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

    //回合开始时的buff调用,以后记得加个死亡检测
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

    //玩家回合循环，也是还没加死亡检测
    public void PlayerTurnCircle()
    {
        CheckVictory();
        if (!isGameOver)
        {
            //行动点归零后切换回合
            if (PlayerActionPoints <= 0)
            {
                //处理回合结束的buff
                BuffOnTurnEnd(playerUnit);
                BuffDuationReduce(playerUnit);

                //重新初始化数据
                EnemyActionPoints = 4;
                currentActUnitIndex = 0;
                currentActUnit = enemyUnit[currentActUnitIndex];
                states = BattleStates.ENEMYTURN;
                battleDialogue.text = "enemy turn now！";

                //设置UI
                battleUI.TurnPanel.GetComponentInChildren<Text>().text = "Enemy Turn";
                battleUI.FadeInAndOut();

                //敌方回合开始
                BuffOnTurnBegin(playerUnit);
                StartCoroutine(EnemyTurnCircle());
                return;
            }

            if (currentActUnit.GetComponent<Character>().isDead == false)
            {
                states = BattleStates.PLAYERTURN;
                battleUI.SetBattleUI(currentActUnit);
                Debug.Log("我方还剩" + PlayerActionPoints + "点行动点");
                currentActUnit = playerUnit[currentActUnitIndex];
                Debug.Log("现在是第" + currentActUnitIndex + "位玩家角色在行动");
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
            //行动点归零后切换回合
            if (EnemyActionPoints == 0)
            {
                BuffDuationReduce(enemyUnit);

                //重新初始化数据
                PlayerActionPoints = 4;
                currentActUnitIndex = 0;
                currentActUnit = playerUnit[currentActUnitIndex];
                states = BattleStates.PLAYERTURN;
                battleDialogue.text = "player turn now！";

                //设置UI
                battleUI.TurnPanel.GetComponentInChildren<Text>().text = "Player Turn";
                battleUI.FadeInAndOut();

                //我方回合开始
                BuffOnTurnBegin(playerUnit);
                PlayerTurnCircle();
                yield break;
            }

            if (currentActUnit.GetComponent<Character>().isDead == false)
            {
                states = BattleStates.ENEMYTURN;
                battleUI.SetBattleUI(currentActUnit);


                Debug.Log("敌人还剩" + EnemyActionPoints + "点行动点");
                currentActUnit = enemyUnit[currentActUnitIndex];

                // 敌人随机选择存活目标和技能
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
            else //若死亡则继续循环找到下一个活着的
            {
                Debug.Log("现在是第" + currentActUnitIndex + "位敌人" + currentActUnit.GetComponent<Character>().name + "已死亡！");
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
        Debug.Log("已点击技能按钮" + currentSkillIndex);
        IsPlayerChooseTarget = true;
    }

    public void OnDialogButton()
    {
        if (states != BattleStates.PLAYERTURN) return;
        Debug.Log("已点击对话按钮");
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
                    Debug.Log("已选中" + currentTargetUnit.GetComponent<Character>().name);
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
                    Debug.Log("已选中" + currentTargetUnit.GetComponent<Character>().name);
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
        battleDialogue.text = attackerCharacter.characterName + "对" + currentTargetUnit.GetComponent<Character>().characterName + "使用了" + attackerCharacter.skills[currentSkillIndex].skillName;

        BuffOnBeforeAction();
        SkillInfo skillInfo = new SkillInfo(attackerCharacter.skills[currentSkillIndex], currentActUnit, currentTargetUnit);
        TurnEvents.Add(skillInfo);
        //这里本来有个setui的函数，后面记得调整一下

        //继续循环
        IsPlayerChooseTarget = false;
        currentActUnitIndex = (currentActUnitIndex + 1) % playerUnit.Length;
        currentActUnit = playerUnit[currentActUnitIndex];
        PlayerActionPoints--;
        PlayerTurnCircle();
    }

    public void EnemyUseSkill()
    {
        Character attackerCharacter = currentActUnit.GetComponent<Character>();
        battleDialogue.text = attackerCharacter.characterName + "对" + currentTargetUnit.GetComponent<Character>().characterName + "使用了" + attackerCharacter.skills[currentSkillIndex].skillName;

        BuffOnBeforeAction();
        SkillInfo skillInfo = new SkillInfo(attackerCharacter.skills[currentSkillIndex], currentActUnit, currentTargetUnit);
        TurnEvents.Add(skillInfo);
        //这里本来有个setui的函数，后面记得调整一下

        //继续循环
        currentActUnitIndex = (currentActUnitIndex + 1) % enemyUnit.Length;
        currentActUnit = enemyUnit[currentActUnitIndex];
        EnemyActionPoints--;
        StartCoroutine(EnemyTurnCircle());
    }

    public void BuffOnAfterUseSkill(GameObject user)
    {
        BuffHandler buffHandler = user.GetComponent<BuffHandler>();
        Debug.Log(user.GetComponent<Character>().characterName+"结算了一次行动后buff！");
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
        // 检查玩家是否全部死亡
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

        // 检查敌人是否全部死亡
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
            Debug.Log("玩家胜利！");
            isGameOver = true;
            LevelDataManager.Instance.LoadUI();
            // 在这里可以添加胜利后的逻辑，比如弹出胜利界面、播放胜利动画等
        }
        else if (allPlayersDead)
        {
            Debug.Log("玩家失败！");
            isGameOver = true;
            // 添加失败后的逻辑，如游戏结束界面、重试选项等
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
            //记得以后加个闪避的回调点
            BloodFloat.instance.ShowDamage("Miss!", damageNow.defender);
            return;

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
    }

}
