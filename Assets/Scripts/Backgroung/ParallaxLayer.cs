using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float parallaxX = 0.5f;

    private Transform[] sprites;
    private float width;

    private Vector3 startPosition;

    void Start()
    {
        InitCamera();
        InitSprites();
        InitWidth();
    }

    void LateUpdate()
    {
        if (cameraTransform == null) return;
        UpdateParallax();
        FollowCameraY();
        HandleInfinite();
    }
    void InitCamera()
    {
        if (cameraTransform == null)
        {
            Camera cam = Camera.main;

            if (cam != null)
                cameraTransform = cam.transform;
        }
    }

    void InitSprites()
    {
        int count = transform.childCount;

        if (count < 2)
        {
            Debug.LogWarning("Il n'y a pas 2 sprite pour chaque layer poto");
            return;
        }

        sprites = new Transform[count];

        for (int i = 0; i < count; i++)
        {
            sprites[i] = transform.GetChild(i);
        }
    }

    void InitWidth()
    {
        SpriteRenderer sr = sprites[0].GetComponent<SpriteRenderer>();

        if (sr == null)
        {
            return;
        }

        width = sr.bounds.size.x;
        startPosition = transform.position;
    }

    void UpdateParallax()
    {
        
        float camX = cameraTransform.position.x;

        float targetX = startPosition.x + camX * parallaxX;

        transform.position = new Vector3(
            targetX,
            transform.position.y,
            transform.position.z
        );
    }

    void FollowCameraY()
    {
        float camY = cameraTransform.position.y;

        transform.position = new Vector3(
            transform.position.x,
            camY,
            transform.position.z
        );
    }

    void HandleInfinite()
    {
        float camX = cameraTransform.position.x;

        foreach (Transform sprite in sprites)
        {
            float distance = camX - sprite.position.x;

            if (Mathf.Abs(distance) >= width)
            {
                Transform other = GetOtherSprite(sprite);

                float newX = other.position.x + width * Mathf.Sign(distance);
                sprite.position = new Vector3(newX, sprite.position.y, sprite.position.z);
            }
        }
    }

    Transform GetOtherSprite(Transform current)
    {
        foreach (Transform t in sprites)
        {
            if (t != current)
                return t;
        }
        return current;
    }
}