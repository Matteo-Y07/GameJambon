using System.Collections;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 2f;

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
    }

    public IEnumerator FadeIn()
    {
        yield return Fade(0f, 1f);
    }

    public IEnumerator FadeOut()
    {
        yield return Fade(1f, 0f);
    }

    private IEnumerator Fade(float start, float end)
    {
        float t = 0f;

        canvasGroup.alpha = start;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = end;
    }
}