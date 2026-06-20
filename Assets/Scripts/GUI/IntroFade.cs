using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IntroFade : MonoBehaviour
{
    [SerializeField] private Image fadeImage;

    public IEnumerator FadeToBlack(float duration)
    {
        yield return FadeAlpha(1f, duration);
    }

    public IEnumerator FadeFromBlack(float duration)
    {
        yield return FadeAlpha(0f, duration);
    }

    public IEnumerator FadeBlackToWhite(float duration)
    {
        Color start = Color.black;
        start.a = 1f;

        Color end = Color.white;
        end.a = 1f;

        float time = 0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            fadeImage.color = Color.Lerp(start, end, time / duration);
            yield return null;
        }

        fadeImage.color = end;
    }

    public IEnumerator FadeWhiteToTransparent(float duration)
    {
        Color start = Color.white;
        start.a = 1f;

        Color end = Color.white;
        end.a = 0f;

        float time = 0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            fadeImage.color = Color.Lerp(start, end, time / duration);
            yield return null;
        }

        fadeImage.color = end;
        fadeImage.gameObject.SetActive(false);
    }

    private IEnumerator FadeAlpha(float targetAlpha, float duration)
    {
        Color c = fadeImage.color;
        float startAlpha = c.a;
        float time = 0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;

            c.a = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            fadeImage.color = c;

            yield return null;
        }

        c.a = targetAlpha;
        fadeImage.color = c;
    }
}