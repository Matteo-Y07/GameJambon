using System;
using System.Collections;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField]
    private float textSpeed = 0.03f;

    private DialogueLine[] currentDialogue;
    private int index;
    private bool isTyping;
    private Coroutine typingCoroutine;

    public bool IsDialogueActive { get; private set; }

    public event Action DialogueEnded;

    private void Awake()
    {
        Debug.Log("[DialogueManager] Awake");

        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("[DialogueManager] Duplicate instance destroyed");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private IEnumerator Start()
    {
        Debug.Log("[DialogueManager] Start (init delay)");
        yield return new WaitForSecondsRealtime(0.5f);

        GameState.CanTriggerDialogue = true;
        Debug.Log("[DialogueManager] CanTriggerDialogue = TRUE");
    }

    private void Update()
    {
        if (!IsDialogueActive)
            return;

        if (Input.GetButtonDown("Submit"))
        {
            Debug.Log("[DialogueManager] Submit pressed");
            ContinueDialogue();
        }
    }

    public void StartDialogue(DialogueLine[] dialogue)
    {
        Debug.Log("[DialogueManager] StartDialogue() called");

        if (dialogue == null || dialogue.Length == 0)
        {
            Debug.LogWarning("[DialogueManager] Invalid dialogue array");
            return;
        }

        if (DialogueUI.Instance == null || DialogueUI.Instance.panel == null)
        {
            Debug.LogWarning("[DialogueManager] UI missing");
            return;
        }

        currentDialogue = dialogue;
        index = 0;

        IsDialogueActive = true;

        Time.timeScale = 0f;

        DialogueUI.Instance.panel.SetActive(true);

        Debug.Log("[DialogueManager] Dialogue STARTED");

        ShowLine();
    }

    private void ShowLine()
    {
        Debug.Log($"[DialogueManager] ShowLine index={index}");

        DialogueUI ui = DialogueUI.Instance;
        DialogueLine line = currentDialogue[index];

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
        {
            Debug.Log("[DialogueManager] Stopping previous typing coroutine");
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeText(line.text));
    }

    private IEnumerator TypeText(string text)
    {
        Debug.Log("[DialogueManager] TypeText START");

        isTyping = true;

        DialogueUI ui = DialogueUI.Instance;
        ui.dialogueText.text = "";

        foreach (char c in text)
        {
            ui.dialogueText.text += c;
            yield return new WaitForSecondsRealtime(textSpeed);
        }

        isTyping = false;

        Debug.Log("[DialogueManager] TypeText END");
    }

    public void ContinueDialogue()
    {
        Debug.Log("[DialogueManager] ContinueDialogue()");

        DialogueUI ui = DialogueUI.Instance;

        if (isTyping)
        {
            Debug.Log("[DialogueManager] Skipping type animation");

            StopCoroutine(typingCoroutine);
            ui.dialogueText.text = currentDialogue[index].text;

            isTyping = false;
            return;
        }

        index++;

        if (index >= currentDialogue.Length)
        {
            Debug.Log("[DialogueManager] End of dialogue reached");
            EndDialogue();
            return;
        }

        ShowLine();
    }

    private void EndDialogue()
    {
        Debug.Log("[DialogueManager] EndDialogue()");

        DialogueUI ui = DialogueUI.Instance;

        if (ui != null && ui.panel != null)
            ui.panel.SetActive(false);

        IsDialogueActive = false;

        Time.timeScale = 1f;

        DialogueEnded?.Invoke();

        Debug.Log("[DialogueManager] Dialogue ENDED event invoked");
    }

    private void OnDestroy()
    {
        Debug.Log("[DialogueManager] OnDestroy");

        StopAllCoroutines();
        Time.timeScale = 1f;
    }
}