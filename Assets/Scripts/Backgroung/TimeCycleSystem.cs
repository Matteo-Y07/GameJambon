using UnityEngine;

public class TimeCycleSystem : MonoBehaviour
{
    [SerializeField] private float cycleDuration = 60f;

    private float time;

    public float GetNormalizedTime()
    {
        return (time % cycleDuration) / cycleDuration;
    }

    void Update()
    {
        time += Time.deltaTime;

    }
}