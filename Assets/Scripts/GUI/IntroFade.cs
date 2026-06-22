using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IntroFade : MonoBehaviour
{
    [SerializeField]
    private Image fadeImage;

    public IEnumerator Fade(
        Color start,
        Color end,
        float duration,
        bool disableAtEnd = false)
    {
        Debug.Log($"[IntroFade] Fade START {start} -> {end} ({duration}s)");

        fadeImage.gameObject.SetActive(true);
        fadeImage.color = start;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            fadeImage.color = Color.Lerp(start, end, elapsed / duration);

            yield return null;
        }

        fadeImage.color = end;

        Debug.Log("[IntroFade] Fade END");

        if (disableAtEnd)
        {
            Debug.Log("[IntroFade] FadeImage disabled");
            fadeImage.gameObject.SetActive(false);
        }
    }
}