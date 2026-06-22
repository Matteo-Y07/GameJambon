using System;
using System.Collections;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] private float textSpeed = 0.03f;

    private DialogueLine[] currentDialogue;
    private int index;
    private bool isTyping;
    private Coroutine typingCoroutine;

    public bool IsDialogueActive { get; private set; }
    public event Action DialogueEnded;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        GameState.CanTriggerDialogue = true;
    }

    void Update()
    {
        if (!IsDialogueActive)
            return;

        if (Input.GetButtonDown("Submit"))
            ContinueDialogue();
    }

    public void StartDialogue(DialogueLine[] dialogue)
    {
        if (!GameState.CanTriggerDialogue)
            return;

        if (dialogue == null || dialogue.Length == 0)
            return;

        if (DialogueUI.Instance == null || DialogueUI.Instance.panel == null)
        {
            Debug.LogError("DialogueUI not ready (missing instance or panel)");
            return;
        }

        currentDialogue = dialogue;
        index = 0;

        IsDialogueActive = true;
        GameState.InDialogue = true;

        DialogueUI.Instance.panel.SetActive(true);

        ShowLine();
    }

    void ShowLine()
    {
        var ui = DialogueUI.Instance;
        var line = currentDialogue[index];

        ui.nameText.text = line.characterName;

        if (line.portrait != null)
        {
            ui.portrait.enabled = true;
            ui.portrait.sprite = line.portrait;
        }
        else
        {
            ui.portrait.enabled = false;
        }

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(line.text));
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;

        var ui = DialogueUI.Instance;
        ui.dialogueText.text = "";

        foreach (char c in text)
        {
            ui.dialogueText.text += c;
            yield return new WaitForSecondsRealtime(textSpeed);
        }

        isTyping = false;
    }

    public void ContinueDialogue()
    {
        var ui = DialogueUI.Instance;

        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            ui.dialogueText.text = currentDialogue[index].text;
            isTyping = false;
            return;
        }

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
        var ui = DialogueUI.Instance;

        if (ui != null && ui.panel != null)
            ui.panel.SetActive(false);

        IsDialogueActive = false;
        GameState.InDialogue = false;

        DialogueEnded?.Invoke();
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }
}