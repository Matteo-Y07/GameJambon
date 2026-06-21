using UnityEngine;
public class DialogueTrigger : MonoBehaviour
{
    public DialogueLine[] dialogue;
    public DialogueManager manager;

    private bool triggered = false;
    private IntroFade introFade;
    void Start()
    {
        if (introFade == null)
        {
            introFade = FindObjectOfType<IntroFade>();
        }
        if (manager == null)
        {
            manager = FindObjectOfType<DialogueManager>();
        }
    }

    

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !triggered)
        {
            triggered = true;

            manager.StartDialogue(dialogue);
        }
    }
}