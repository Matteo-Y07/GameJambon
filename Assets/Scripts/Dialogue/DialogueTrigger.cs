using UnityEngine;
public class DialogueTrigger : MonoBehaviour
{
    public DialogueLine[] dialogue;
    public DialogueManager manager;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !triggered)
        {
            triggered = true;

            manager.StartDialogue(dialogue);
        }
    }
}