using System.Collections;
using UnityEngine;

public class IntroSequence : MonoBehaviour
{
    [SerializeField] private IntroFade introFade;
    [SerializeField] private DialogueManager dialogueManager;

    [SerializeField] private DialogueLine[] dialogue1;
    [SerializeField] private DialogueLine[] dialogue2;    


    private void Start()
    {
        StartCoroutine(PlayIntro());
    }

    private IEnumerator PlayIntro()
    {
        // 1. S'assurer écran noir au départ
        yield return introFade.FadeToBlack(0f);

        // 2. Premier dialogue
        dialogueManager.StartDialogue(dialogue1);
        yield return new WaitUntil(() => !dialogueManager.IsDialogueActive);

        // 3. Fade noir → blanc
        yield return introFade.FadeBlackToWhite(1.5f);

        // 4. Blanc → transparent (révéler jeu)
        yield return introFade.FadeWhiteToTransparent(1.5f);

        // 5. Deuxième dialogue
        dialogueManager.StartDialogue(dialogue2);
    }
}