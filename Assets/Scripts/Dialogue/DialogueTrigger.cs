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
    }

    void Update()
    {
        Debug.Log("Intro fade status: " + (introFade != null ? introFade.intro.ToString() : "No IntroFade found"));
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