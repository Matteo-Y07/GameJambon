using System.Collections;
using UnityEngine;

public class IntroSequence : MonoBehaviour
{
    [SerializeField] private IntroFade introFade;
    [SerializeField] private DialogueManager dialogueManager;

    [SerializeField] private DialogueLine[] dialogue1;
    [SerializeField] private DialogueLine[] dialogue2;
    [SerializeField] private PlayerMovement player;


    private void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerMovement>();
        }
        StartCoroutine(PlayIntro());
    }

    private IEnumerator PlayIntro()
    {
        // 1. S'assurer écran noir au départ
        player.enabled = false;
        yield return introFade.Fade(Color.clear, Color.black, 0f);

        // 2. Premier dialogue
        dialogueManager.StartDialogue(dialogue1);
        yield return new WaitUntil(() => !dialogueManager.IsDialogueActive);

        // 3. Fade noir → blanc
        yield return introFade.Fade(Color.black, Color.white, 1.5f);

        // 4. Blanc → transparent (révéler jeu)
        yield return introFade.Fade(Color.white, Color.clear, 1.5f);

        // 5. Deuxième dialogue
        dialogueManager.StartDialogue(dialogue2);
        yield return new WaitUntil(() => !dialogueManager.IsDialogueActive);

        // 6. Réactiver le player pour le gameplay
        player.enabled = true;
    }
}