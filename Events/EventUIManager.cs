using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventUIManager : MonoBehaviour
{
    public GameObject eventPanel; // 事件UI面板
    public Text titleText; // 事件标题文本
    public Text descriptionText; // 事件描述文本
    public Image eventImage; // 事件图片
    public Button[] buttons; // UI上的按钮数组
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
        descriptionText.text = currentEvent.pages[currentPageIndex].descriptionText; // 显示对应文本
        //eventImage.sprite = currentEvent.image;

        // 动态设置按钮
        for (int i = 0; i < Mathf.Min(buttons.Length, currentEvent.pages[currentPageIndex].eventButtons.Length); i++)
        {
            buttons[i].gameObject.SetActive(true);
            buttons[i].GetComponentInChildren<Text>().text = currentEvent.pages[currentPageIndex].eventButtons[i].buttonText;
            int index = i;
            buttons[i].onClick.AddListener(() => OnButtonClick(index));
        }

        // 隐藏多余的按钮
        for (int i = currentEvent.pages[currentPageIndex].eventButtons.Length; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
    }

    private void OnButtonClick(int index)
    {
        EventButton action = currentEvent.pages[currentPageIndex].eventButtons[index];
        //先检测有没有触发的事件，有的按钮既触发事件又推进页面
        if (action.buttonActive != null)
        {
            // 执行回调操作，比如修改角色属性
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
            // 推进到下一个事件
            StartEvent(action.nextEvent);
        }
    }

    private void CloseEvent()
    {
        eventPanel.SetActive(false);
    }
}
