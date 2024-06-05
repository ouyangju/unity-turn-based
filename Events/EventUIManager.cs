using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventUIManager : MonoBehaviour
{
    public GameObject eventPanel; // �¼�UI���
    public Text titleText; // �¼������ı�
    public Text descriptionText; // �¼������ı�
    public Image eventImage; // �¼�ͼƬ
    public Button[] buttons; // UI�ϵİ�ť����
    public EventData eventTo;
    private int currentPageIndex = 0;
    private EventData currentEvent;

    private void Start()
    {
        currentEvent = eventTo;
        UpdateUI();
    }

    public void StartEvent(EventData eventToStart)
    {
        currentEvent = eventToStart;
        UpdateUI();
    }

    private void UpdateUI()
    {
        //titleText.text = currentEvent.title;
        descriptionText.text = currentEvent.pages[currentPageIndex].descriptionText; // ��ʾ��Ӧ�ı�
        //eventImage.sprite = currentEvent.image;

        // ��̬���ð�ť
        for (int i = 0; i < Mathf.Min(buttons.Length, currentEvent.pages[currentPageIndex].eventButtons.Length); i++)
        {
            buttons[i].gameObject.SetActive(true);
            buttons[i].GetComponentInChildren<Text>().text = currentEvent.pages[currentPageIndex].eventButtons[i].buttonText;
            int index = i;
            buttons[i].onClick.AddListener(() => OnButtonClick(index));
        }

        // ���ض���İ�ť
        for (int i = currentEvent.pages[currentPageIndex].eventButtons.Length; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
    }

    private void OnButtonClick(int index)
    {
        EventButton action = currentEvent.pages[currentPageIndex].eventButtons[index];
        //�ȼ����û�д������¼����еİ�ť�ȴ����¼����ƽ�ҳ��
        if (action.buttonActive != null)
        {
            // ִ�лص������������޸Ľ�ɫ����
            action.buttonActive.ApplyEvent();
            if (action.isEndEvent)
            {
                CloseEvent();
            }
        }

        if (action.isNextPage)
        {
            currentPageIndex = action.targetPage;
            UpdateUI();
        }
        else if (action.nextEvent != null)
        {
            // �ƽ�����һ���¼�
            StartEvent(action.nextEvent);
        }
    }

    private void CloseEvent()
    {
        eventPanel.SetActive(false);
    }
}
