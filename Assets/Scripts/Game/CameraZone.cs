using UnityEngine;

public class CameraZone : MonoBehaviour
{
    public float minX;
    public float maxX;

    private void Awake()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();

        minX = box.bounds.min.x;
        maxX = box.bounds.max.x;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CameraFollow cam =
                Camera.main.GetComponent<CameraFollow>();

            cam.SetBounds(minX, maxX);
        }
    }
}