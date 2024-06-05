using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RewardManager : MonoBehaviour
{
    public static RewardManager instance;
    public SkillData[] allSkills;
    public Button[] chooseSkillButtons;
    public GameObject[] playerCharacter;
    public int buttonsPerCharacter = 3;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        for (int i = 0; i < LevelDataManager.Instance.playerCharacter.Length; i++)
        {
            playerCharacter[i] = LevelDataManager.Instance.playerCharacter[i];
        }

        foreach (Button button in chooseSkillButtons)
        {
            button.gameObject.SetActive(false);
        }

        CreateChooseSkillButton();
    }

    public void LoadSkillReward()
    {
        CreateChooseSkillButton();
    }

    private void CreateChooseSkillButton()
    {
        for (int i = 0; i < playerCharacter.Length; i++)
        {
            if (playerCharacter[i] == null)
            {
                continue;
            }
            //指派相应的获取技能方法给对应按钮
            List<SkillData> selectedSkills = SelectRandomSkills(playerCharacter[i].GetComponent<Character>().Profess);
            int startIndex = i * buttonsPerCharacter;
            for (int j = 0; j < buttonsPerCharacter; j++)
            {
                Button button = chooseSkillButtons[startIndex + j];
                SkillData skill = selectedSkills[j];
                button.onClick.RemoveAllListeners();
                int currentIndex = i;
                button.onClick.AddListener(() => OnChooseSkillButton(currentIndex, skill));
                button.GetComponentInChildren<Text>().text = skill.skillName;
                button.gameObject.SetActive(true);
            }
        }
    }

    private List<SkillData> SelectRandomSkills(Profess profess)
    {
        //找到符合职业的技能
        List<SkillData> filteredSkills = new List<SkillData>();
        foreach (SkillData skill in allSkills)
        {
            if (skill.skillProfess == profess)
            {
                filteredSkills.Add(skill);
            }
        }

        //筛选三个不重复的随机技能
        List<SkillData> result = new List<SkillData>();
        while (result.Count < 3 && filteredSkills.Count > 0)
        {
            int index = Random.Range(0, filteredSkills.Count);
            result.Add(filteredSkills[index]);
            filteredSkills.RemoveAt(index);
        }
        return result;
    }

    public void OnChooseSkillButton(int characterIndex, SkillData skill)
    {
        Character character = playerCharacter[characterIndex].GetComponent<Character>();
        if (character != null)
        {
            character.LeranSkill(skill);
        }

        foreach (Button button in chooseSkillButtons)
        {
            button.gameObject.SetActive(false);
        }
    }
}
