using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public Text[] skillButton;

    public GameObject playerPanel;
    public Text buffTexts;
    public float offset;

    private GameObject currentTargetUnit;
    private Ray targetChooseRay;            //���ѡ��Ŀ�������
    private RaycastHit targetHit;           //����Ŀ��

    private void Awake()
    {
        playerPanel.SetActive(false);
        TurnPanel.SetActive(false);
    }

    public void SetBattleUI(GameObject character)
    {

        Character skillowner = character.GetComponent<Character>();
        for (int i = 0; i < skillButton.Length; i++)
        {
            if (skillowner.skills[i] == null)
            {
                skillButton[i].text = "�޼���";
                continue;
            }
            skillButton[i].text = skillowner.skills[i].skillName;
            skillButton[i].transform.parent.GetComponent<Button>().GetComponent<SkillInfoDisplay>().data = skillowner.skills[i];
            AddEventTrigger(skillButton[i].transform.parent.GetComponent<Button>(), skillowner.skills[i]);
        }
        SetPlayerStatesPanel(character);
    }

    private void AddEventTrigger(Button button, SkillData skillData)
    {
        SkillInfoDisplay skillInfoDisplay = button.GetComponent<SkillInfoDisplay>();
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = button.gameObject.AddComponent<EventTrigger>();
        }

        // �Ƴ��Ѵ��ڵ�PointerEnter�¼�  
        trigger.triggers.RemoveAll(entry => entry.eventID == EventTriggerType.PointerEnter);

        // ���PointerEnter�¼�
        EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
        pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
        pointerEnterEntry.callback.AddListener((data) => { skillInfoDisplay.ShowSkillInfo(skillData); });
        trigger.triggers.Add(pointerEnterEntry);

        // �Ƴ��Ѵ��ڵ�PointerExit�¼�
        trigger.triggers.RemoveAll(entry => entry.eventID == EventTriggerType.PointerExit);

        // ���PointerExit�¼�
        EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry();
        pointerExitEntry.eventID = EventTriggerType.PointerExit;
        pointerExitEntry.callback.AddListener((data) => { skillInfoDisplay.HideSkillInfo(); });
        trigger.triggers.Add(pointerExitEntry);
    }


    private void Update()
    {
        targetChooseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(targetChooseRay, out targetHit))
        {
            if (targetHit.collider.gameObject.tag == "EnemyUnit")
            {
                currentTargetUnit = targetHit.collider.gameObject;
                playerPanel.SetActive(true);
                playerPanel.transform.position = new Vector3(currentTargetUnit.transform.position.x + offset, currentTargetUnit.transform.position.y, currentTargetUnit.transform.position.z);
                BuffHandler buffHandler = currentTargetUnit.GetComponent<BuffHandler>();
                UpdateBuffPanel(buffHandler);
            }
            else
            {
                playerPanel.SetActive(false);
                HideBuffPanel();
            }
        }
        else
        {
            playerPanel.SetActive(false);
            HideBuffPanel();
        }
    }

    public void UpdateBuffPanel(BuffHandler buffHandler)
    {
        int index = 0;
        buffTexts.text = null;
        foreach (var buffInfo in buffHandler.buffList)
        {
            // ����ֻ��һ���򵥵�ʾ��������Ը�����Ҫ��ʾBuffInfo�е���������  
            buffTexts.text += "Buff Name: " + buffInfo.buffData.buffName + " " + buffInfo.currentStack + "��\n" + "ʣ��غ���" + buffInfo.remainTurn + "\n" + buffInfo.buffData.description + "\n";
            index++;
        }
    }

    public void HideBuffPanel()
    {
        if (buffTexts != null)
        {
            buffTexts.text = " ";
        }
    }

    public GameObject TurnPanel; // Ҫ���Ƶ�Text���
    public float fadeDuration = 1f; // ����򵭳�����ʱ��

    // ����Text�ĵ���͵���
    public void FadeInAndOut()
    {
        TurnPanel.SetActive(true);
        StartCoroutine(FadeText());
    }

    private IEnumerator FadeText()
    {
        yield return Fade(0f, 1f, fadeDuration); // ����Text
        yield return new WaitForSeconds(1f);
        // ����Text
        yield return Fade(1f, 0f, fadeDuration);
        TurnPanel.SetActive(false);
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        Color color = TurnPanel.GetComponent<Image>().color;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, timer / duration);
            TurnPanel.GetComponent<Image>().color = color;
            yield return null; // �ȴ���һ֡
        }
    }

    public Text playerStatesPanel;

    public void SetPlayerStatesPanel(GameObject character)
    {
        Character statesOwner = character.GetComponent<Character>();
        if (statesOwner != null)
        {
            playerStatesPanel.text = "��ɫ����" + statesOwner.characterName +
                "\nHP:" + statesOwner.currentHp + "/" + statesOwner.maxHp +
                "\nMP:" + statesOwner.currentMp + "/" + statesOwner.maxMp +
                "\n��������" + statesOwner.attack +
                "\n��������" + statesOwner.defense;
        }
    }
}

