using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum BattleStates { START, PLAYERTURN, ENEMYTURN, END, WIN, LOST }

public class TurnBattle : MonoBehaviour
{
    public GameObject gameManager;

    //�������˵�Ԥ����
    public GameObject[] playerPrefabs;
    public GameObject[] enemyPrefabs;

    //�������˵�λ��
    public Transform[] playerBattleStations;
    public Transform[] enemyBattleStations;

    //�������˵�ʵ����������������
    private CharacterStatus[] playerUnit;
    private CharacterStatus[] enemyUnit;

    //ս���ı�
    public Text dialogue;

    //����HUD,��ʱ��ǵ�����ר�ŵ�UI manager��
    public GameObject[] playerHudPrefab;
    public GameObject[] enemyHudPrefab;

    private UnitHUD[] playerHud;
    private UnitHUD[] enemyHud;
    public BattleHud battleHud;

    public BattleStates states; //�غ��Ƶ�״̬
    public int ActionPoints; //�ж����� 
    private CharacterStatus currentActUnit;   //��ǰ�ж��ĵ�λ
    private int currentActUnitIndex;

    private GameObject currentTargetUnit;
    private bool IsPlayerChooseTarget = false;
    private Ray targetChooseRay;            //���ѡ�񹥻����������
    private RaycastHit targetHit;           //����Ŀ��


    //���ܣ���ʱ�ȷ����
    public AbstractSkill Fire;
    public AbstractSkill Ice;
    AbstractSkill Thunder;
    public BuffData buffData;

    // Start is called before the first frame update
    void Start()
    {
        states = BattleStates.START;
        Debug.Log("�ֵ���һغ�");
        StartCoroutine(SetUpBattle());
    }

    //��ʼ��ս��������
    IEnumerator SetUpBattle()
    {


        //��ʼ��˫��������
        playerUnit = new CharacterStatus[playerBattleStations.Length];
        enemyUnit = new CharacterStatus[enemyBattleStations.Length];

        playerHud = new UnitHUD[playerBattleStations.Length];
        enemyHud = new UnitHUD[enemyBattleStations.Length];

        //��������
        SetplayerData(gameManager.GetComponent<PlayerDataManager>().playerCurrentUnit);

        //���ɵ�λ
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
            dialogue.text = "enemy turn now��";
            return;
        }

        states = BattleStates.PLAYERTURN;

        Debug.Log("��ʣ" + ActionPoints + "���ж���");
        currentActUnit = playerUnit[currentActUnitIndex];
        Debug.Log("�����ǵ�" + currentActUnitIndex + "λ��ҽ�ɫ���ж�");
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

        Debug.Log("�ѵ�����ܰ�ť");
        IsPlayerChooseTarget = true;
    }

    public void UseSkill(int skillIndex)
    {
        Debug.Log("��ɫ��ʼʹ�ü���");
        CharacterStatus AttackReceiver = currentTargetUnit.GetComponent<CharacterStatus>();
        currentActUnit.characterSkill[skillIndex].SkillActivate(currentActUnit.attack, AttackReceiver);
        //���ﱾ���и�setui�ĺ���������ǵõ���һ��

        //����ѭ��
        IsPlayerChooseTarget = false;
        currentActUnitIndex = (currentActUnitIndex + 1) % playerUnit.Length;
        ActionPoints--;
        PlayerTurnCircle();
    }

    public void SetplayerData(GameObject[] playerData)
    {
        playerPrefabs = playerData;
    }
    //������ҽ�ɫ
    public void SetPlayerUnits()
    {
        if (playerPrefabs.Length != playerBattleStations.Length)
        {
            Debug.Log("��ҽ�ɫ�����ͳ��������Բ��ϣ�");
            return;
        }
        for (int i = 0; i < playerPrefabs.Length; i++)
        {
            //ʵ�������
            GameObject playerGameObject = Instantiate(playerPrefabs[i], playerBattleStations[i]);
            playerUnit[i] = playerGameObject.GetComponent<CharacterStatus>();
            Debug.Log("�����õ�" + i + "λ" + playerUnit[i]);

            //ʵ������ҽ���
            GameObject playerHudGO = Instantiate(playerHudPrefab[i], playerBattleStations[i]);
            playerHudGO.transform.SetParent(GameObject.Find("Canvas").transform, true);
            playerHud[i] = playerHudGO.GetComponent<UnitHUD>();
            Debug.Log("�����õ�" + i + "λ" + playerHud[i]);

            playerHud[i].setUI(playerUnit[i]);
        }
    }

    //�������˽�ɫ
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
        Debug.Log("��Ϊ" + currentActUnitIndex + "��Ӽ���");
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
                Debug.Log("��ʼѡ��Ŀ��");
                if (Input.GetMouseButtonDown(0) && targetHit.collider.gameObject.tag == "EnemyUnit")
                {
                    currentTargetUnit = targetHit.collider.gameObject;
                    Debug.Log("��ѡ��" + currentTargetUnit.GetComponent<CharacterStatus>().name);
                    UseSkill(0);
                }
            }
        }
        enemyHud[0].setUI(enemyUnit[0]);
        enemyHud[1].setUI(enemyUnit[1]);
    }
}
