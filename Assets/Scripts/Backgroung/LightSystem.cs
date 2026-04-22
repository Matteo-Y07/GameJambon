using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSystem : MonoBehaviour
{
    [SerializeField] private Light2D globalLight;
    [SerializeField] private TimeCycleSystem timeSystem;

    [SerializeField] private Color dayColor;
    [SerializeField] private Color nightColor;

    void Update()
    {
        float t = Mathf.Sin(timeSystem.GetNormalizedTime() * Mathf.PI);

        globalLight.color = Color.Lerp(nightColor, dayColor, t);
        globalLight.intensity = Mathf.Lerp(0.3f, 1f, t);
    }
}