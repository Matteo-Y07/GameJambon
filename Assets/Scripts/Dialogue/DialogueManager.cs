using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;

    public TMP_Text nameText;
    public TMP_Text dialogueText;

    public Image portraitImage;

    private DialogueLine[] currentDialogue;
    private int index;

    private bool isTyping;

    void Update()
    {
        if(dialoguePanel.activeSelf && Input.GetButtonDown("Submit"))
        {
            // Si le texte est encore en train de s'écrire
            // on le termine instantanément
            ContinueDialogue();
        }
    }

    public void StartDialogue(DialogueLine[] dialogue)
    {
        dialoguePanel.SetActive(true);

        currentDialogue = dialogue;
        index = 0;

        ShowLine();
    }

    void ShowLine()
    {
        DialogueLine line = currentDialogue[index];

        nameText.text = line.characterName;
        if (line.portrait != null)
        {
            portraitImage.sprite = line.portrait;
        }
        else {
            GetComponent<Image>().enabled = false;
        }

        StopAllCoroutines();
        StartCoroutine(TypeLine(line.text));
    }

    void NextLine()
    {
        index++;

        if(index >= currentDialogue.Length)
        {
            dialoguePanel.SetActive(false);
            return;
        }

        ShowLine();
    }

    IEnumerator TypeLine(string text)
    {
        isTyping = true;

        dialogueText.text = "";

        foreach(char c in text)
        {
            dialogueText.text += c;

            yield return new WaitForSeconds(0.03f);
        }

        isTyping = false;
    }

    public void ContinueDialogue()
    {
        if (!dialoguePanel.activeSelf)
            return;

        if (isTyping)
        {
            StopAllCoroutines();

            dialogueText.text = currentDialogue[index].text;
            isTyping = false;
        }
        else
        {
            NextLine();
        }
    }
}