using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private GameObject[] backgrounds;
    [SerializeField] private float cycleDuration = 60f;

    private float time;
    private int currentIndex = -1;

    void Update()
    {
        if (backgrounds == null || backgrounds.Length == 0)
        {
            Debug.LogError("BackgroundManager: No backgrounds assigned!");
            return;
        }

        time += Time.deltaTime;

        float t = (time % cycleDuration) / cycleDuration;

        int index = Mathf.FloorToInt(t * backgrounds.Length);
        index = Mathf.Clamp(index, 0, backgrounds.Length - 1);

        if (index != currentIndex)
        {
            SwitchBackground(index);
        }

    }

    void SwitchBackground(int index)
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            if (backgrounds[i] != null)
                backgrounds[i].SetActive(i == index);
        }
        Debug.Log("Switch to: " + backgrounds[index].name);

        for (int i = 0; i < backgrounds.Length; i++)
        {
            Debug.Log(backgrounds[i].name + " active = " + (i == index));
        }
        currentIndex = index;
    }
}