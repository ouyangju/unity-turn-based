using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEditor;

public class LevelDataManager : MonoBehaviour
{
    public static LevelDataManager Instance;
    public GameObject[] playerPrefabs;
    //玩家数据
    public GameObject[] playerCharacter;
    public int coin;
    public List<BlessingData> blessingDatas = new List<BlessingData>();

    private string[] sceneNames = { "Event", "Enemy", "Shop", "Elite", "Boss", "BlackMarket", "Rest", "Upgrade" };
    private Dictionary<string, int> sceneWeights = new Dictionary<string, int>
    {
        {"Event", 5},
        {"Enemy", 7},
        {"Shop", 3},
        {"Elite", 2},
        {"Boss", 1},
        {"BlackMarket", 6},
        {"Rest", 4},
        {"Upgrade", 5}
    };

    public Button leaveButton;
    public Button[] newButtons;
    public int sceneSelectionCount = 0; // 计数器

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        leaveButton.gameObject.SetActive(false);
        foreach (Button button in newButtons)
        {
            button.gameObject.SetActive(false);
        }
        InitializedCharacter();
    }

    public void InitializedCharacter()
    {

        for (int i = 0; i < playerPrefabs.Length; i++)
        {
            if (playerPrefabs[i] == null)
            {
                continue;
            }
            playerPrefabs[i].GetComponent<Character>().CopyDataTo(playerCharacter[i].GetComponent<Character>());
        }

    }

    public void LoadUI()
    {
        InitializeUI();
    }



    public void InitializeUI()
    {
        leaveButton.gameObject.SetActive(true);
        leaveButton.onClick.RemoveAllListeners();
        leaveButton.onClick.AddListener(OnLeaveButtonClick);

        foreach (Button button in newButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    public void OnLeaveButtonClick()
    {
        leaveButton.gameObject.SetActive(false);
        GenerateNewSceneButtonsWithConstraints();
    }

    private void GenerateNewSceneButtonsWithConstraints()
    {
        List<string> availableScenes = new List<string>(sceneNames);
        if (sceneSelectionCount == 8)
        {
            // 第9次选择，至少有一个按钮是"Shop"
            availableScenes.Remove("Shop");
            AssignButton("Shop");
            availableScenes.Add("Shop");
        }
        else if (sceneSelectionCount == 9)
        {
            // 第10次选择，只有"Boss"场景
            foreach (Button button in newButtons)
            {
                AssignButton("Boss");
            }
            sceneSelectionCount++; // 增加计数器后不再继续分配其他场景
            return;
        }

        while (availableScenes.Count > 0 && newButtons.Any(button => button.gameObject.activeSelf == false))
        {
            string selectedScene = SelectWeightedRandomScene(availableScenes);
            AssignButton(selectedScene);
            availableScenes.Remove(selectedScene);
        }
        sceneSelectionCount++;
    }

    private string SelectWeightedRandomScene(List<string> scenes)
    {
        int totalWeight = scenes.Sum(scene => sceneWeights[scene]);
        int randomValue = Random.Range(0, totalWeight) + 1;
        int weightSum = 0;

        foreach (string scene in scenes)
        {
            weightSum += sceneWeights[scene];
            if (randomValue <= weightSum)
            {
                return scene;
            }
        }

        // 应该不会走到这一步，但作为安全措施返回一个默认值
        return scenes[0];
    }

    private void AssignButton(string sceneName)
    {
        Button button = newButtons.FirstOrDefault(b => b.gameObject.activeSelf == false);
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnNewButtonClick(sceneName));
            button.GetComponentInChildren<Text>().text = sceneName;
            button.gameObject.SetActive(true);
        }
    }

    public void OnNewButtonClick(string newSceneName)
    {
        Debug.Log($"New button clicked! Scene: {newSceneName}");
        SceneManager.LoadScene(newSceneName);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (var blessingInfo in blessingDatas)
        {
            blessingInfo.OnSwitchLevel?.ApplyBlessinng();
        }

        Debug.Log($"Scene loaded: {scene.name}");
        leaveButton.gameObject.SetActive(false);
        foreach (Button button in newButtons)
        {
            button.gameObject.SetActive(false);
        }
    }
}