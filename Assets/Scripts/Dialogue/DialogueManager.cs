using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Image portraitImage;

    [Header("Settings")]
    [SerializeField] private float textSpeed = 0.03f;

    private DialogueLine[] currentDialogue;
    private int index;
    private bool isTyping;
    public bool IsDialogueActive { get; private set; }
    public event Action DialogueEnded;

    void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (GameState.InPause)
            return;

        if (dialoguePanel.activeSelf && Input.GetButtonDown("Submit"))
        {
            ContinueDialogue();
        }
    }

    public void StartDialogue(DialogueLine[] dialogue)
    {
        GameState.InDialogue = true;
        IsDialogueActive = true;

        Time.timeScale = 0f;

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
            portraitImage.enabled = true;
            portraitImage.sprite = line.portrait;
        }
        else
        {
            portraitImage.enabled = false;
        }

        StopAllCoroutines();
        StartCoroutine(TypeLine(line.text));
    }

    void NextLine()
    {
        index++;

        if (index >= currentDialogue.Length)
        {
            EndDialogue();
            return;
        }

        ShowLine();
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);

        GameState.InDialogue = false;
        IsDialogueActive = false;
        Time.timeScale = 1f;

        DialogueEnded?.Invoke();
    }

    IEnumerator TypeLine(string text)
    {
        isTyping = true;

        dialogueText.text = "";

        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSecondsRealtime(textSpeed);
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