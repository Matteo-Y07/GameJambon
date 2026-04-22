using UnityEngine;

public class MonsterPollution : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 2f;

    [Header("Pollution")]
    [SerializeField] private GarbageBar garbageBar;
    [SerializeField] private float pollutionPerSecond = 5f;

    private Transform player;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
    }

    void Update()
    {
        if (player == null) return;

        // Move toward player
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        // Pollution only if visible
        if (IsVisible())
        {
            garbageBar.Add(pollutionPerSecond * Time.deltaTime);
        }
    }

    bool IsVisible()
    {
        return rend != null && rend.isVisible;
    }
}