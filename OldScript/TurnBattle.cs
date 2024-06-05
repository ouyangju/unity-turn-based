using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum BattleStates { START, PLAYERTURN, ENEMYTURN, END, WIN, LOST }

public class TurnBattle : MonoBehaviour
{
    public GameObject gameManager;

    //玩家与敌人的预制体
    public GameObject[] playerPrefabs;
    public GameObject[] enemyPrefabs;

    //玩家与敌人的位置
    public Transform[] playerBattleStations;
    public Transform[] enemyBattleStations;

    //玩家与敌人的实例化对象的属性组件
    private CharacterStatus[] playerUnit;
    private CharacterStatus[] enemyUnit;

    //战斗文本
    public Text dialogue;

    //各类HUD,到时候记得做个专门的UI manager类
    public GameObject[] playerHudPrefab;
    public GameObject[] enemyHudPrefab;

    private UnitHUD[] playerHud;
    private UnitHUD[] enemyHud;
    public BattleHud battleHud;

    public BattleStates states; //回合制的状态
    public int ActionPoints; //行动点数 
    private CharacterStatus currentActUnit;   //当前行动的单位
    private int currentActUnitIndex;

    private GameObject currentTargetUnit;
    private bool IsPlayerChooseTarget = false;
    private Ray targetChooseRay;            //玩家选择攻击对象的射线
    private RaycastHit targetHit;           //射线目标


    //技能（暂时先放这里）
    public AbstractSkill Fire;
    public AbstractSkill Ice;
    AbstractSkill Thunder;
    public BuffData buffData;

    // Start is called before the first frame update
    void Start()
    {
        states = BattleStates.START;
        Debug.Log("轮到玩家回合");
        StartCoroutine(SetUpBattle());
    }

    //初始化战斗的数据
    IEnumerator SetUpBattle()
    {


        //初始化双方的人数
        playerUnit = new CharacterStatus[playerBattleStations.Length];
        enemyUnit = new CharacterStatus[enemyBattleStations.Length];

        playerHud = new UnitHUD[playerBattleStations.Length];
        enemyHud = new UnitHUD[enemyBattleStations.Length];

        //传入数据
        SetplayerData(gameManager.GetComponent<PlayerDataManager>().playerCurrentUnit);

        //生成单位
        SetPlayerUnits();
        SetEnemyUnits();
        dialogue.text = "battle start!";

        currentActUnitIndex = 0;
        currentActUnit = playerUnit[currentActUnitIndex];

        yield return new WaitForSeconds(1f);
        ActionPoints = 4;
        PlayerTurnCircle();
    }

    public void PlayerTurnCircle()
    {

        if (ActionPoints == 0)
        {
            states = BattleStates.ENEMYTURN;
            dialogue.text = "enemy turn now！";
            return;
        }

        states = BattleStates.PLAYERTURN;

        Debug.Log("还剩" + ActionPoints + "点行动点");
        currentActUnit = playerUnit[currentActUnitIndex];
        Debug.Log("现在是第" + currentActUnitIndex + "位玩家角色在行动");
        battleHud.SetBattleHud(currentActUnit);

        IsPlayerTurn();
    }

    public void EnemyTurnCircle()
    {
        if (ActionPoints == 0)
        {
            states = BattleStates.PLAYERTURN;
            return;
        }

        states = BattleStates.ENEMYTURN;
    }

    bool IsPlayerTurn()
    {
        if (states == BattleStates.PLAYERTURN)
        {
            dialogue.text = "choose your skills";
            return true;
        }
        else return false;
    }

    public void OnSkillButton()
    {
        if (states != BattleStates.PLAYERTURN) return;

        Debug.Log("已点击技能按钮");
        IsPlayerChooseTarget = true;
    }

    public void UseSkill(int skillIndex)
    {
        Debug.Log("角色开始使用技能");
        CharacterStatus AttackReceiver = currentTargetUnit.GetComponent<CharacterStatus>();
        currentActUnit.characterSkill[skillIndex].SkillActivate(currentActUnit.attack, AttackReceiver);
        //这里本来有个setui的函数，后面记得调整一下

        //继续循环
        IsPlayerChooseTarget = false;
        currentActUnitIndex = (currentActUnitIndex + 1) % playerUnit.Length;
        ActionPoints--;
        PlayerTurnCircle();
    }

    public void SetplayerData(GameObject[] playerData)
    {
        playerPrefabs = playerData;
    }
    //创建玩家角色
    public void SetPlayerUnits()
    {
        if (playerPrefabs.Length != playerBattleStations.Length)
        {
            Debug.Log("玩家角色数量和场地数量对不上！");
            return;
        }
        for (int i = 0; i < playerPrefabs.Length; i++)
        {
            //实例化玩家
            GameObject playerGameObject = Instantiate(playerPrefabs[i], playerBattleStations[i]);
            playerUnit[i] = playerGameObject.GetComponent<CharacterStatus>();
            Debug.Log("已设置第" + i + "位" + playerUnit[i]);

            //实例化玩家界面
            GameObject playerHudGO = Instantiate(playerHudPrefab[i], playerBattleStations[i]);
            playerHudGO.transform.SetParent(GameObject.Find("Canvas").transform, true);
            playerHud[i] = playerHudGO.GetComponent<UnitHUD>();
            Debug.Log("已设置第" + i + "位" + playerHud[i]);

            playerHud[i].setUI(playerUnit[i]);
        }
    }

    //创建敌人角色
    public void SetEnemyUnits()
    {
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            GameObject enemyGameObject = Instantiate(enemyPrefabs[i], enemyBattleStations[i]);
            enemyUnit[i] = enemyGameObject.GetComponent<CharacterStatus>();

            GameObject enemyHudGO = Instantiate(enemyHudPrefab[i], enemyBattleStations[i]);
            enemyHudGO.transform.SetParent(GameObject.Find("Canvas").transform, true);
            enemyHud[i] = enemyHudGO.GetComponent<UnitHUD>();

            enemyHud[i].setUI(enemyUnit[i]);
        }
    }


    public void PlayerLearnSkills()
    {
        currentActUnit.AddSkill(Fire);
        Debug.Log("已为" + currentActUnitIndex + "添加技能");
        battleHud.SetBattleHud(currentActUnit);
    }

    public void PlayerLearnIce()
    {
        currentActUnit.AddSkill(Ice);
        battleHud.SetBattleHud(currentActUnit);
    }

    public void PlayerAddBuff()
    {
        BuffInfo buffInfo = new BuffInfo(buffData, currentTargetUnit);
        currentActUnit.GetComponent<BuffHandler>().AddBuff(buffInfo);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlayerChooseTarget)
        {
            targetChooseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(targetChooseRay, out targetHit))
            {
                Debug.Log("开始选择目标");
                if (Input.GetMouseButtonDown(0) && targetHit.collider.gameObject.tag == "EnemyUnit")
                {
                    currentTargetUnit = targetHit.collider.gameObject;
                    Debug.Log("已选中" + currentTargetUnit.GetComponent<CharacterStatus>().name);
                    UseSkill(0);
                }
            }
        }
        enemyHud[0].setUI(enemyUnit[0]);
        enemyHud[1].setUI(enemyUnit[1]);
    }
}
