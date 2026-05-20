using UnityEngine;

public class CatFollower : MonoBehaviour
{
    public Transform player;

    [Header("Movement")]
    public float followSpeed = 6f;
    public float minDistance = 1.5f;
    public float teleportDistance = 8f;

    [Header("Smoothness")]
    public float smoothTime = 0.15f;

    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;

        float distance = Vector2.Distance(transform.position, player.position);

        // Si le chat est trop loin → téléport (évite de le perdre)
        if (distance > teleportDistance)
        {
            transform.position = player.position;
            return;
        }

        // Si proche → reste un peu en idle
        if (distance < minDistance)
            return;

        // Position cible légèrement décalée (effet naturel)
        Vector3 targetPosition = player.position + new Vector3(-1f, 0.5f, 0f);

        // Mouvement smooth
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime,
            followSpeed
        );
    }
}