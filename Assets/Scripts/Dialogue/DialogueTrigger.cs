using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueLine[] dialogue;

    private bool triggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (triggered)
            return;

        if (!GameState.CanTriggerDialogue)
            return;

        if (GameState.InDialogue)
            return;

        if (DialogueManager.Instance == null)
            return;

        triggered = true;

        DialogueManager.Instance.StartDialogue(dialogue);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        triggered = false;
    }
}