using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 5f;

    private float minX;
    private float maxX;
    private Camera cam;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        cam = GetComponent<Camera>();

        if (player != null)
        {
            target = player.transform;
            // Position immédiate sur le joueur au début
            transform.position = new Vector3(target.position.x, target.position.y, -10f);
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // On suit directement le joueur
        float targetX = target.position.x;
        float targetY = target.position.y;

        // Clamp dans les limites de la zone
        targetX = Mathf.Clamp(targetX, minX + cam.orthographicSize*1.5f, maxX - cam.orthographicSize*1.5f);
        Vector3 targetPosition = new Vector3(targetX, targetY, -10f);
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }

    public void SetBounds(float newMinX, float newMaxX)
    {
        minX = newMinX;
        maxX = newMaxX;
    }
}