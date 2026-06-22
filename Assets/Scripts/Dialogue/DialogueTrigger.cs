using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]
    private DialogueLine[] dialogue;

    private bool triggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("[DialogueTrigger] OnTriggerEnter2D");

        if (!other.CompareTag("Player"))
        {
            Debug.Log("[DialogueTrigger] Not player");
            return;
        }

        if (triggered)
        {
            Debug.Log("[DialogueTrigger] Already triggered");
            return;
        }

        if (!GameState.CanTriggerDialogue)
        {
            Debug.Log("[DialogueTrigger] Blocked by GameState");
            return;
        }

        if (DialogueManager.Instance == null)
        {
            Debug.LogWarning("[DialogueTrigger] No DialogueManager instance");
            return;
        }

        if (DialogueManager.Instance.IsDialogueActive)
        {
            Debug.Log("[DialogueTrigger] Dialogue already active");
            return;
        }

        triggered = true;

        Debug.Log("[DialogueTrigger] START dialogue");
        DialogueManager.Instance.StartDialogue(dialogue);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        Debug.Log("[DialogueTrigger] Player exited trigger");

        triggered = false;
    }

    private void OnDisable()
    {
        Debug.Log("[DialogueTrigger] OnDisable reset triggered");
        triggered = false;
    }
}