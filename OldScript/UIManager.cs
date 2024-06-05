using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    private string[] sceneNames = new string[] { "BattleScene", "EventScene", "RecoverScene", "UpgradeScene", "ShopScene" };


    // �뿪��ť
    public Button leaveButton;

    // �°�ť
    public Button[] newButtons;

    // ���п��ܵ��¼��б�
    public string[] events;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // ��ӳ���������ɵ��¼�����
        SceneManager.sceneLoaded += OnSceneLoaded;

        leaveButton.gameObject.SetActive(true);
        // ����뿪��ť����¼�
        leaveButton.onClick.AddListener(OnLeaveButtonClick);

        // ��ʼ���°�ť
        foreach (Button button in newButtons)
        {
            string nextScene = RandomSceneName(sceneNames);
            // ����°�ť����¼�
            button.onClick.AddListener(() => OnNewButtonClick(nextScene));
            button.GetComponentInChildren<Text>().text = nextScene;
            button.gameObject.SetActive(false);
        }
    }

    // �뿪��ť����¼�
    public void OnLeaveButtonClick()
    {
        // �����뿪��ť
        leaveButton.gameObject.SetActive(false);

        // ��ʾ�°�ť
        foreach (Button button in newButtons)
        {
            button.gameObject.SetActive(true);
        }
    }

    // �°�ť����¼�
    public void OnNewButtonClick(string newSceneName)
    {
        // ���������ִ���°�ť�����Ĳ���
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
        Debug.Log("�����������: " + scene.name);

        leaveButton.gameObject.SetActive(true);
        // �Ƴ����е��¼�������뿪��ť����¼�
        leaveButton.onClick.RemoveAllListeners();
        leaveButton.onClick.AddListener(OnLeaveButtonClick);

        // ��ʼ���°�ť
        foreach (Button button in newButtons)
        {
            button.onClick.RemoveAllListeners();
            string nextScene = RandomSceneName(sceneNames);
            // ����°�ť����¼�
            button.onClick.AddListener(() => OnNewButtonClick(nextScene));
            button.GetComponentInChildren<Text>().text = nextScene;
            button.gameObject.SetActive(false);
        }
    }
}
