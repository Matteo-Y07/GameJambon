using System.Collections;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private IntroFade introFade;

    public void PlayCutscene()
    {
        StartCoroutine(CutsceneRoutine());
    }

    private IEnumerator CutsceneRoutine()
    {
        // 1. écran noir
        yield return introFade.Fade(Color.clear, Color.black, 1f);

        // 2. dialogue 1
        DialogueLine[] dialogue1 = GetDialogue1();
        dialogueManager.StartDialogue(dialogue1);

        yield return new WaitUntil(() => !GameState.InDialogue);

        // 3. fade out écran noir (on “révèle” le jeu)
        yield return introFade.Fade(Color.black, Color.clear, 1f);

        // 4. dialogue 2
        DialogueLine[] dialogue2 = GetDialogue2();
        dialogueManager.StartDialogue(dialogue2);

        yield return new WaitUntil(() => !GameState.InDialogue);
    }

    private DialogueLine[] GetDialogue1()
    {
        return new DialogueLine[]
        {
            new DialogueLine { characterName = "???", text = "..." },
            new DialogueLine { characterName = "???", text = "Where am I ?" }
        };
    }

    private DialogueLine[] GetDialogue2()
    {
        return new DialogueLine[]
        {
            new DialogueLine { characterName = "Guide", text = "Wake up." }
        };
    }
}