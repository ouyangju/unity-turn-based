using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Dialogue dialogue;
    private int index;

    Text dialogueContent;
    Text dialogueName;
    Image dialogueImage;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void StartDialogue()
    {
        gameObject.SetActive(true);
    }
    private void OnEnable()
    {
        dialogue = TurnManager.instance.dialogue;
        dialogueContent = transform.Find("ÄÚÈÝ").GetComponent<Text>();
        dialogueName = transform.Find("Ãû×Ö").GetComponent<Text>();
        dialogueImage = transform.Find("Í·Ïñ").GetComponent<Image>();

        index = 0;
        PlayDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && dialogue != null)
        {
            if (index == dialogue.dialogueNodes.Length)
            {
                gameObject.SetActive(false);
                index = 0;
            }
            else
            {
                PlayDialogue();
            }
        }
    }

    private void PlayDialogue()
    {
        DialogueNode node = dialogue.dialogueNodes[index++];

        dialogueContent.text = node.dialogueContent;
        dialogueName.text = node.speakerName;
        dialogueImage.sprite = node.speakerSprite;
    }
}
