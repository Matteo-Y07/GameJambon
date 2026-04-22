using UnityEngine;
using System.Collections;

public class BackgroundSystem : MonoBehaviour
{
    [SerializeField] private GameObject[] backgrounds;
    [SerializeField] private float cycleDuration = 120f;
    [SerializeField] private float fadeDuration = 2f;

    private float time;
    private int currentIndex = -1;

    private SpriteRenderer[][] groups;
    private Coroutine fadeCoroutine;

    void Start()
    {
        groups = new SpriteRenderer[backgrounds.Length][];

        for (int i = 0; i < backgrounds.Length; i++)
        {
            groups[i] = backgrounds[i].GetComponentsInChildren<SpriteRenderer>();

            float alpha = (i == 0) ? 1f : 0f;

            foreach (var sr in groups[i])
                SetAlpha(sr, alpha);
        }

        currentIndex = 0;
    }

    void Update()
    {
        if (backgrounds == null || backgrounds.Length == 0)
            return;

        time += Time.deltaTime;

        float t = (time % cycleDuration) / cycleDuration;

        int index = Mathf.FloorToInt(t * backgrounds.Length);
        index = Mathf.Clamp(index, 0, backgrounds.Length - 1);

        if (index != currentIndex)
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            fadeCoroutine = StartCoroutine(Crossfade(currentIndex, index));
            currentIndex = index;
        }
    }

    IEnumerator Crossfade(int from, int to)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;

            foreach (var sr in groups[from])
                SetAlpha(sr, 1f - t);

            foreach (var sr in groups[to])
                SetAlpha(sr, t);

            yield return null;
        }

        foreach (var sr in groups[from])
            SetAlpha(sr, 0f);

        foreach (var sr in groups[to])
            SetAlpha(sr, 1f);
    }

    void SetAlpha(SpriteRenderer sr, float alpha)
    {
        Color c = sr.color;
        c.a = alpha;
        sr.color = c;
    }
}