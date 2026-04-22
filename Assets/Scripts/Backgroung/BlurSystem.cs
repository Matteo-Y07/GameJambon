using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlurSystem : MonoBehaviour
{
    [SerializeField] private Volume volume;
    [SerializeField] private TimeCycleSystem timeSystem;

    private DepthOfField dof;

    void Start()
    {
        volume.profile.TryGet(out dof);
    }

    void Update()
    {
        if (dof == null) return;

        float t = timeSystem.GetNormalizedTime();

        // blur léger la nuit / transitions
        float blur = Mathf.Abs(Mathf.Sin(t * Mathf.PI));

        dof.focalLength.value = Mathf.Lerp(0f, 200f, blur);
    }
}