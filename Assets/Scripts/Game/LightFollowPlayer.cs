using UnityEngine;

public class LightFollowPlayer : MonoBehaviour
{
    private Transform player;

    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -1f);
    [SerializeField] private float smoothSpeed = 10f;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
    }

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 targetPos = player.position + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            smoothSpeed * Time.deltaTime
        );
    }
}