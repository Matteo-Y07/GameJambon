using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IntroFade : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    public IEnumerator Fade(Color start, Color end, float duration, bool disableAtEnd = false)
    {
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

        if (disableAtEnd)
            fadeImage.gameObject.SetActive(false);
    }
}