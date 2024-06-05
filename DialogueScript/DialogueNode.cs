using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueNode
{
    [Header("��ɫ������")]
    public string speakerName;
    [Header("��ɫ��ͼƬ")]
    public Sprite speakerSprite;
    [Header("�Ի����������")]
    public int dialogueIndex;//�Ի���index��������ת�Ի�

    [TextArea, Header("�Ի�����")]
    public string dialogueContent;

}
