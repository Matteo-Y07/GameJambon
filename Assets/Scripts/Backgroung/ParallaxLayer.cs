using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float parallaxFactor = 0.5f;

    private float startX;
    private float startZ;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        startX = transform.position.x;
        startZ = transform.position.z;
    }

    void LateUpdate()
    {
        float cameraX = cameraTransform.position.x;
        float cameraY = cameraTransform.position.y;

        transform.position = new Vector3(
            startX + (cameraX - startX) * parallaxFactor, // parallax X
            cameraY,                                      // follow caméra Y
            startZ                                        // Z fixe
        );
    }
}