using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Image portrait;
    public GameObject panel;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Validate();

        if (panel != null)
            panel.SetActive(false);
    }

    void Validate()
    {
        if (panel == null)
            Debug.LogError("DialogueUI: panel NOT assigned");

        if (nameText == null)
            Debug.LogError("DialogueUI: nameText NOT assigned");

        if (dialogueText == null)
            Debug.LogError("DialogueUI: dialogueText NOT assigned");

        if (portrait == null)
            Debug.LogError("DialogueUI: portrait NOT assigned");
    }
}