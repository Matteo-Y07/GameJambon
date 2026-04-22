using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlurSystem : MonoBehaviour
{
    [SerializeField] private Volume volume;

    private DepthOfField dof;

    void Start()
    {
        volume.profile.TryGet(out dof);

        if (dof != null)
        {
            dof.mode.value = DepthOfFieldMode.Gaussian;
        }
    }

    public void SetBlur(float value)
    {
        if (dof == null) return;

        value = Mathf.Clamp01(value);

        //URP Gaussian DOF
        dof.gaussianStart.value = Mathf.Lerp(0f, 5f, value);
        dof.gaussianEnd.value = Mathf.Lerp(10f, 0.5f, value);
        Camera.main.orthographicSize = Mathf.Lerp(5f, 6f, value);
    }
}