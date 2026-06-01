using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroFade : MonoBehaviour
{
    public Image fadeImage;
    public PlayerMovement playerMovement;
    public bool intro = true;

    private void Start()
    {
        StartCoroutine(IntroSequence());
        playerMovement.enabled = false;

    }

    IEnumerator IntroSequence()
    {
        yield return StartCoroutine(Fade(Color.black, Color.white, 2f));

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(Fade(Color.white, new Color(1,1,1,0), 2f));
    }

    IEnumerator Fade(Color start, Color end, float duration)
    {
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            fadeImage.color = Color.Lerp(start, end, t / duration);
            yield return null;
        }

        fadeImage.color = end;
        if (end.a == 0)
        {
            playerMovement.enabled = true;
            intro = false;
        }
    }
}