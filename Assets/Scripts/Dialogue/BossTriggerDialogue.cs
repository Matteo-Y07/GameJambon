using UnityEngine;

public class BossTriggerDialogue : MonoBehaviour
{
    [Header("Dialogue")]
    public DialogueLine[] dialogue;
    public DialogueManager manager;

    [Header("Boss")]
    public BossTuto bossToActivate;

    private bool triggered = false;
    private IntroFade introFade;

    void Start()
    {
        if (introFade == null)
        {
            introFade = FindObjectOfType<IntroFade>();
        }

        if (manager != null)
        {
            manager.DialogueEnded += OnDialogueEnded;
        }
        else
        {
            manager = FindObjectOfType<DialogueManager>();
            if (manager != null)
            {
                manager.DialogueEnded += OnDialogueEnded;
            }
        }
        if (bossToActivate == null)
        {
            bossToActivate = FindObjectOfType<BossTuto>();
        }
    }

    private void OnDestroy()
    {
        if (manager != null)
        {
            manager.DialogueEnded -= OnDialogueEnded;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            manager.StartDialogue(dialogue);
        }
    }

    private void OnDialogueEnded()
    {
        if (bossToActivate != null)
        {
            bossToActivate.ActivateBoss();
        }
    }
}