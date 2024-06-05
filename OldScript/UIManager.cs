using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    private string[] sceneNames = new string[] { "BattleScene", "EventScene", "RecoverScene", "UpgradeScene", "ShopScene" };


    // 离开按钮
    public Button leaveButton;

    // 新按钮
    public Button[] newButtons;

    // 所有可能的事件列表
    public string[] events;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // 添加场景加载完成的事件监听
        SceneManager.sceneLoaded += OnSceneLoaded;

        leaveButton.gameObject.SetActive(true);
        // 添加离开按钮点击事件
        leaveButton.onClick.AddListener(OnLeaveButtonClick);

        // 初始化新按钮
        foreach (Button button in newButtons)
        {
            string nextScene = RandomSceneName(sceneNames);
            // 添加新按钮点击事件
            button.onClick.AddListener(() => OnNewButtonClick(nextScene));
            button.GetComponentInChildren<Text>().text = nextScene;
            button.gameObject.SetActive(false);
        }
    }

    // 离开按钮点击事件
    public void OnLeaveButtonClick()
    {
        // 隐藏离开按钮
        leaveButton.gameObject.SetActive(false);

        // 显示新按钮
        foreach (Button button in newButtons)
        {
            button.gameObject.SetActive(true);
        }
    }

    // 新按钮点击事件
    public void OnNewButtonClick(string newSceneName)
    {
        // 在这里可以执行新按钮点击后的操作
        Debug.Log("New button clicked!");
        SceneManager.LoadScene(newSceneName);

    }

    private string RandomSceneName(string[] strings)
    {
        int selectedIndex = Random.Range(0, strings.Length);
        return strings[selectedIndex];
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("场景加载完成: " + scene.name);

        leaveButton.gameObject.SetActive(true);
        // 移除已有的事件，添加离开按钮点击事件
        leaveButton.onClick.RemoveAllListeners();
        leaveButton.onClick.AddListener(OnLeaveButtonClick);

        // 初始化新按钮
        foreach (Button button in newButtons)
        {
            button.onClick.RemoveAllListeners();
            string nextScene = RandomSceneName(sceneNames);
            // 添加新按钮点击事件
            button.onClick.AddListener(() => OnNewButtonClick(nextScene));
            button.GetComponentInChildren<Text>().text = nextScene;
            button.gameObject.SetActive(false);
        }
    }
}
