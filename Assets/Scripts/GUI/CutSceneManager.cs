using System.Collections;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private IntroFade introFade;

    public void PlayCutscene()
    {
        Debug.Log("[CutsceneManager] PlayCutscene() called");
        StartCoroutine(CutsceneRoutine());
    }

    private IEnumerator CutsceneRoutine()
    {
        Debug.Log("[CutsceneManager] CutsceneRoutine START");

        // 1. écran noir
        Debug.Log("[CutsceneManager] Fade to black");
        yield return introFade.Fade(Color.clear, Color.black, 1f);
        Debug.Log("[CutsceneManager] Fade to black DONE");

        // 2. dialogue 1
        Debug.Log("[CutsceneManager] Starting Dialogue 1");
        DialogueLine[] dialogue1 = GetDialogue1();
        dialogueManager.StartDialogue(dialogue1);

        yield return new WaitUntil(() =>
        {
            bool active = DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive;
            return !active;
        });

        Debug.Log("[CutsceneManager] Dialogue 1 finished");

        // 3. fade out écran noir
        Debug.Log("[CutsceneManager] Fade to clear");
        yield return introFade.Fade(Color.black, Color.clear, 1f);
        Debug.Log("[CutsceneManager] Fade to clear DONE");

        // 4. dialogue 2
        Debug.Log("[CutsceneManager] Starting Dialogue 2");
        DialogueLine[] dialogue2 = GetDialogue2();
        dialogueManager.StartDialogue(dialogue2);

        yield return new WaitUntil(() =>
        {
            bool active = DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive;
            return !active;
        });

        Debug.Log("[CutsceneManager] Dialogue 2 finished");
        Debug.Log("[CutsceneManager] CutsceneRoutine END");
    }

    private DialogueLine[] GetDialogue1()
    {
        Debug.Log("[CutsceneManager] GetDialogue1()");
        return new DialogueLine[]
        {
            new DialogueLine { characterName = "???", text = "..." },
            new DialogueLine { characterName = "???", text = "Where am I ?" }
        };
    }

    private DialogueLine[] GetDialogue2()
    {
        Debug.Log("[CutsceneManager] GetDialogue2()");
        return new DialogueLine[]
        {
            new DialogueLine { characterName = "Guide", text = "Wake up." }
        };
    }
}