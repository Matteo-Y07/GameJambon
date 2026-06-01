using TMPro;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public TMP_Text tutorialText;

    [TextArea]
    public string message;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialText.text = message;
            tutorialText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialText.gameObject.SetActive(false);
        }
    }
}