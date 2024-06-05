using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueNode
{
    [Header("角色的名字")]
    public string speakerName;
    [Header("角色的图片")]
    public Sprite speakerSprite;
    [Header("对话的相关属性")]
    public int dialogueIndex;//对话的index，用来跳转对话

    [TextArea, Header("对话内容")]
    public string dialogueContent;

}
